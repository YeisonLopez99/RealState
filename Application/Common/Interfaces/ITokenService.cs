using System.Security.Claims;

namespace Application.Common.Interfaces;

public record TokenResult(string Token, string TokenHash, DateTime Expires);

public interface ITokenService
{
    string CreateAccessToken(Guid userId, IEnumerable<Claim>? extraClaims = null);
    TokenResult GenerateRefreshToken();
}