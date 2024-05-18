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
            if (context.Session.GetInt32("userId") == null)
            {
                _logger.LogInformation("(Middleware) Try create user session");

                var accessToken = context.Request.Cookies["jwtCookie"];
                var refreshToken = context.Request.Cookies["refreshTokenCookie"];
                var claimsPrincipal = tokenService.GetPrincipalFromExpiredToken(accessToken);
                int userId = Convert.ToInt32(claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "id").Value);

                var userCompany = dbContext.UserCompanies.FirstOrDefault(u => u.UserId == userId);

                _logger.LogInformation("(Middleware) access token: {token}", accessToken);
                _logger.LogInformation("(Middleware) refresh token: {token}", refreshToken);
                _logger.LogInformation("(Middleware) user id: {id}", userId);
                _logger.LogInformation("(Middleware) company id: {id}", userId);

                context.Session.SetInt32("userId", userId);
                if (userCompany != null)
                    context.Session.SetInt32("companyId", userCompany.CompanyId);

                await _next.Invoke(context);
                return;
            }
            
            await _next.Invoke(context);
        }
    }
}