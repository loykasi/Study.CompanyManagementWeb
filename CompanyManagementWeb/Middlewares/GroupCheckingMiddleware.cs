using System.Security.Claims;
using CompanyManagementWeb.Data;
using CompanyManagementWeb.DataAccess;
using CompanyManagementWeb.Services;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagementWeb.Middlewares
{
    public class GroupCheckingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<KeepLoginMiddleware> _logger;

        public GroupCheckingMiddleware(RequestDelegate next, ILogger<KeepLoginMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, ITokenService tokenService, CompanyManagementDbContext dbContext)
        {
            var companyId = context.Session.GetInt32(SessionVariable.CompanyId);
            var userId = context.Session.GetInt32(SessionVariable.UserId);
            if (companyId != null && userId != null)
            {
                if (!dbContext.UserCompanies.Any(u => u.UserId == userId))
                {
                    DeleteCompanyFromSession(context);
                }
            }
            
            await _next.Invoke(context);
        }

        private void DeleteCompanyFromSession(HttpContext context)
        {
            context.Session.Remove(SessionVariable.CompanyId);
            context.Session.Remove(SessionVariable.CompanyName);
        }
    }
}