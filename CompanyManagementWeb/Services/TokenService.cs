using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CompanyManagementWeb.DataAccess;
using CompanyManagementWeb.Models;
using Microsoft.IdentityModel.Tokens;

namespace CompanyManagementWeb.Services
{
    public class TokenService : ITokenService
    {
        private readonly CompanyManagementDbContext _context;
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration, CompanyManagementDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public Token GenerateToken(User user)
        {
            return new Token
            {
                AccessToken = GenerateAccessToken(user),
                RefreshToken = GenerateRefreshToken()
            };
        }

        public string GenerateAccessToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claim = new Claim[]
            {
                new Claim("id", user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: credentials,
                claims: claim
                );
 
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public bool ValidateAccessToken(string token)
        {
            if (token == null) 
                return false;

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(
                    token, 
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.GetSection("Jwt:Key").Value!)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    },
                    out SecurityToken validatedToken);

                // var jwtToken = (JwtSecurityToken)validatedToken;
                // var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var Key = Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value!);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken 
                    || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        public bool IsRefreshTokenValid(string refreshToken)
        {
            var user = _context.Users.FirstOrDefault(u => u.RefreshToken == refreshToken);
            if (user == null)
                return false;
            return true;
        }

        public void SetJWTCookie(HttpContext httpContext, string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(3),
            };
            httpContext.Response.Cookies.Append("jwtCookie", token, cookieOptions);
        }

        public void SetRefreshTokenCookie(HttpContext httpContext, int id, string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7),
            };
            httpContext.Response.Cookies.Append("refreshTokenCookie", token, cookieOptions);

            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            user.RefreshToken = token;
            _context.SaveChanges();
        }
    }
}