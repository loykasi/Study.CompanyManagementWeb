using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CompanyManagementWeb.Authentication
{
    public class JwtHelper
    {
        public static string GenerateJSONWebToken(IConfiguration configuration)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("Jwt:Key").Value!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
 
            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
 
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static bool ValidateToken(IConfiguration configuration, string token)
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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetSection("Jwt:Key").Value!)),
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
    }    
}