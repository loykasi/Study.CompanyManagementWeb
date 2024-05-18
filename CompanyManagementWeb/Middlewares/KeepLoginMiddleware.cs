using System.Security.Claims;
using CompanyManagementWeb.DataAccess;
using CompanyManagementWeb.Services;

namespace CompanyManagementWeb.Middlewares
{
    public class KeepLoginMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<KeepLoginMiddleware> _logger;

        public KeepLoginMiddleware(RequestDelegate next, ILogger<KeepLoginMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, ITokenService tokenService, CompanyManagementDbContext dbContext)
        {
            if (context.Session.GetInt32("loginStatus") == null && context.Session.GetInt32("loginStatus") != 0)
            {
                _logger.LogInformation("(Middleware) Try create user session");

                var accessToken = context.Request.Cookies["jwtCookie"];
                var refreshToken = context.Request.Cookies["refreshTokenCookie"];
                int userId = 0;

                if (!tokenService.ValidateAccessToken(accessToken))
                {
                    if (tokenService.IsRefreshTokenValid(refreshToken))
                    {
                        if (tokenService.TryGetPrincipalFromToken(accessToken, out ClaimsPrincipal claims))
                        {
                            userId = Convert.ToInt32(claims.Claims.FirstOrDefault(c => c.Type == "id").Value);
                            var tokens = tokenService.GenerateToken(new Models.User { Id = userId });
                            tokenService.SetJWTCookie(context, tokens.AccessToken!);
                            tokenService.SetRefreshTokenCookie(context, userId, tokens.RefreshToken!);
                        }
                        else
                        {
                            // context.Response.Redirect("/Identity/Login");
                            context.Session.SetInt32("loginStatus", 0);
                            context.Response.Cookies.Delete("jwtCookie");
                            context.Response.Cookies.Delete("refreshTokenCookie");
                            await _next.Invoke(context);
                            return;
                        }
                    }
                    else
                    {
                        // context.Response.Redirect("/Identity/Login");
                        context.Session.SetInt32("loginStatus", 0);
                        context.Response.Cookies.Delete("jwtCookie");
                        context.Response.Cookies.Delete("refreshTokenCookie");
                        await _next.Invoke(context);
                        return;
                    }
                }

                var userCompany = dbContext.UserCompanies.FirstOrDefault(u => u.UserId == userId);

                _logger.LogInformation("(Middleware) access token: {token}", accessToken);
                _logger.LogInformation("(Middleware) refresh token: {token}", refreshToken);
                _logger.LogInformation("(Middleware) user id: {id}", userId);
                _logger.LogInformation("(Middleware) company id: {id}", userId);

                context.Session.SetInt32("userId", userId);
                if (userCompany != null)
                    context.Session.SetInt32("companyId", userCompany.CompanyId);
                context.Session.SetInt32("loginStatus", 1);

                await _next.Invoke(context);
                return;
            }
            
            await _next.Invoke(context);
        }
    }
}