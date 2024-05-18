using CompanyManagementWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CompanyManagementWeb.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class JwtAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // string token = context.HttpContext.Session.GetString("Token");
            string token = context.HttpContext.Request.Cookies["jwtCookie"]!;

            if (string.IsNullOrEmpty(token))
            {
                System.Diagnostics.Debug.WriteLine("Redirect", "(AUTHENTICATION)");
                context.Result = new RedirectToActionResult("Login", "Identity", null);
                return;
            }

            ITokenService tokenService = context.HttpContext.RequestServices.GetRequiredService<ITokenService>();

            if (!tokenService.ValidateAccessToken(token))
            {
                System.Diagnostics.Debug.WriteLine("Jwt token invalid", "(AUTHENTICATION)");
                string refreshToken = context.HttpContext.Request.Cookies["refreshTokenCookie"]!;
                if(tokenService.IsRefreshTokenValid(refreshToken))
                {
                    var tokens = tokenService.GenerateToken(new Models.User { Id = context.HttpContext.Session.GetInt32("userId").Value });
                    tokenService.SetJWTCookie(context.HttpContext, tokens.AccessToken!);
                    tokenService.SetRefreshTokenCookie(context.HttpContext, context.HttpContext.Session.GetInt32("userId").Value, tokens.RefreshToken!);
                    System.Diagnostics.Debug.WriteLine("Refresh token successful", "(AUTHENTICATION)");
                    return;
                }

                System.Diagnostics.Debug.WriteLine("Token invalid, login again", "(AUTHENTICATION)");
                context.Result = new RedirectToActionResult("Login", "Identity", null);
                return;
            }


            System.Diagnostics.Debug.WriteLine("Authorize successful", "(AUTHENTICATION)");
        }
    }
}