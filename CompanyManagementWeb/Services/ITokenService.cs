using System.Security.Claims;
using CompanyManagementWeb.Models;

namespace CompanyManagementWeb.Services
{
    public interface ITokenService
    {
        public Token GenerateToken(User user);
        // public string GenerateAccessToken();
        // public string GenerateRefreshToken();
        public bool ValidateAccessToken(string token);
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

        public bool IsRefreshTokenValid(string refreshToken);
        public void SetJWTCookie(HttpContext httpContext, string token);
        public void SetRefreshTokenCookie(HttpContext httpContext, int id, string token);
    }
}