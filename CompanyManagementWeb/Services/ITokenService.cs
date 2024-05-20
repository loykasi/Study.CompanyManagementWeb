using System.Security.Claims;
using CompanyManagementWeb.Models;

namespace CompanyManagementWeb.Services
{
    public interface ITokenService
    {
        public Token GenerateToken(User user);
        public bool ValidateAccessToken(string token);
        public bool TryGetPrincipalFromToken(string token, out ClaimsPrincipal? claims);

        public Task<bool> IsRefreshTokenValid(string refreshToken);
        public void SetJWTCookie(HttpContext httpContext, string token);
        public void SetRefreshTokenCookie(HttpContext httpContext, int id, string token);
    }
}