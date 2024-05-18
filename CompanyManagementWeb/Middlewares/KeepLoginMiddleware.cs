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

        public async Task Invoke(HttpContext context)
        {
            if (context.Session.GetInt32("userId") == null)
            {
                _logger.LogInformation("(Middleware) Try create user session");

                var accessToken = context.Request.Cookies["jwtCookie"];
                var refreshToken = context.Request.Cookies["refreshTokenCookie"];
                _logger.LogInformation("(Middleware) access token: {token}", accessToken);
                _logger.LogInformation("(Middleware) refresh token: {token}", refreshToken);

                await _next.Invoke(context);
                return;
            }
            
            await _next.Invoke(context);
        }
    }
}