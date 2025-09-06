using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Security;

public class JwtTokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly byte[] _key;

    public JwtTokenService(IConfiguration config)
    {
        _config = config;
        var secret = _config["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key not configured");
        _key = Encoding.UTF8.GetBytes(secret);
    }

    public string CreateAccessToken(Guid userId, IEnumerable<Claim>? extraClaims = null)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        if (extraClaims != null) claims.AddRange(extraClaims);

        var creds = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpiresMinutes"] ?? "15"));

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public TokenResult GenerateRefreshToken()
    {
        var random = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(random);
        var token = Convert.ToBase64String(random);

        using var sha = SHA256.Create();
        var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(token));
        var tokenHash = Convert.ToBase64String(hash);

        var expires = DateTime.UtcNow.AddDays(int.Parse(_config["Jwt:RefreshTokenTtlDays"] ?? "30"));
        return new TokenResult(token, tokenHash, expires);
    }
}