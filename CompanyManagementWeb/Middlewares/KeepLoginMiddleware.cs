using System.Security.Claims;
using CompanyManagementWeb.DataAccess;
using CompanyManagementWeb.Services;
using Microsoft.EntityFrameworkCore;

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
            if (context.Session.GetInt32("loginStatus") == null || context.Session.GetInt32("loginStatus") != 1)
            {
                _logger.LogInformation("(Middleware) Try create user session");

                var accessToken = context.Request.Cookies["jwtCookie"];
                var refreshToken = context.Request.Cookies["refreshTokenCookie"];
                int userId = 0;

                if (accessToken == null && refreshToken == null)
                {
                    context.Session.SetInt32("loginStatus", 1);
                    await _next.Invoke(context);
                    return;
                }

                ClaimsPrincipal? claims = null;

                if (!tokenService.ValidateAccessToken(accessToken))
                {
                    if (await tokenService.IsRefreshTokenValid(refreshToken))
                    {
                        if (tokenService.TryGetPrincipalFromToken(accessToken, out claims))
                        {
                            userId = Convert.ToInt32(claims.Claims.FirstOrDefault(c => c.Type == "id").Value);
                            var tokens = tokenService.GenerateToken(new Models.User { Id = userId });
                            tokenService.SetJWTCookie(context, tokens.AccessToken!);
                            tokenService.SetRefreshTokenCookie(context, userId, tokens.RefreshToken!);
                        }
                        else
                        {
                            StopTryToLogin(context);
                            await _next.Invoke(context);
                            return;
                        }
                    }
                    else
                    {
                        StopTryToLogin(context);
                        await _next.Invoke(context);
                        return;
                    }
                }

                if (claims != null)
                {
                    userId = Convert.ToInt32(claims.Claims.FirstOrDefault(c => c.Type == "id").Value);
                }
                else
                {
                    if (tokenService.TryGetPrincipalFromToken(accessToken, out claims))
                    {
                        userId = Convert.ToInt32(claims.Claims.FirstOrDefault(c => c.Type == "id").Value);
                    }
                    else
                    {
                        StopTryToLogin(context);
                        await _next.Invoke(context);
                        return;
                    }
                }

                // _logger.LogInformation("(Middleware) access token: {token}", accessToken);
                // _logger.LogInformation("(Middleware) refresh token: {token}", refreshToken);
                // _logger.LogInformation("(Middleware) user id: {id}", userId);
                // _logger.LogInformation("(Middleware) company id: {id}", userId);

                
                context.Session.SetInt32("userId", userId);
                context.Session.SetInt32("loginStatus", 1);

                var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null)
                    context.Session.SetString("userName", user.Name);

                var userCompany = await dbContext.UserCompanies.Include(u => u.Company).FirstOrDefaultAsync(u => u.UserId == userId);
                if (userCompany != null)
                {
                    context.Session.SetInt32("companyId", userCompany.CompanyId);
                    context.Session.SetString("companyId", userCompany.Company.Name);
                }

                await _next.Invoke(context);
                return;
            }
            
            await _next.Invoke(context);
        }

        private void StopTryToLogin(HttpContext context)
        {
            context.Session.SetInt32("loginStatus", 1);
            context.Response.Cookies.Delete("jwtCookie");
            context.Response.Cookies.Delete("refreshTokenCookie");
        }
    }
}