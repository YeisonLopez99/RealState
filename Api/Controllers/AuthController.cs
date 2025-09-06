using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;


/// <summary>
/// Autoriza el consumo del api
/// </summary>
[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly ITokenService _tokens;

    public AuthController(ApplicationDbContext db, ITokenService tokens)
    {
        _db = db;
        _tokens = tokens;
    }

    /// <summary>
    /// Autoriza y genera token para consumir el api.
    /// </summary>
    /// <param name="Username">Usuario para la autenticación.</param>
    /// <param name="Password">Contraseña para la autenticación.</param>
    /// <returns>Token de autenticación y token de refresco</returns>

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        var user = await _db.Owners.FirstOrDefaultAsync(o => o.Email == req.Username);
        if (user == null) return Unauthorized();

        if (!VerifyPassword(req.Password, user.PasswordHash)) return Unauthorized();

        var access = _tokens.CreateAccessToken(user.IdOwner);
        var rt = _tokens.GenerateRefreshToken();

        _db.RefreshTokens.Add(new Domain.Entities.RefreshToken { UserId = user.IdOwner, TokenHash = rt.TokenHash, Expires = rt.Expires });
        await _db.SaveChangesAsync();

        return Ok(new { accessToken = access, refreshToken = rt.Token });
    }

    /// <summary>
    /// Refresca token de autorización.
    /// </summary>
    /// <param name="RefreshToken">Token de refresco.</param>
    /// <returns>Nuevo token de autorización y nuevo token de refresco</returns>
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.RefreshToken)) return BadRequest();

        var hash = HashToken(req.RefreshToken);
        var stored = await _db.RefreshTokens.FirstOrDefaultAsync(x => x.TokenHash == hash);
        if (stored == null || stored.Revoked || stored.Expires < DateTime.UtcNow) return Unauthorized();

        // rotate
        stored.Revoked = true;
        var newRt = _tokens.GenerateRefreshToken();
        _db.RefreshTokens.Add(new Domain.Entities.RefreshToken { UserId = stored.UserId, TokenHash = newRt.TokenHash, Expires = newRt.Expires });
        await _db.SaveChangesAsync();

        var access = _tokens.CreateAccessToken(stored.UserId);
        return Ok(new { accessToken = access, refreshToken = newRt.Token });
    }

    private static string HashToken(string token)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(bytes);
    }

    private static bool VerifyPassword(string password, string stored)
    {
        try
        {
            var parts = Convert.FromBase64String(stored);
            var salt = parts.Take(16).ToArray();
            var hash = parts.Skip(16).ToArray();
            using var derive = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
            var computed = derive.GetBytes(hash.Length);
            return CryptographicOperations.FixedTimeEquals(computed, hash);
        }
        catch { return false; }
    }
}

public record LoginRequest(string Username, string Password);
public record RefreshRequest(string RefreshToken);