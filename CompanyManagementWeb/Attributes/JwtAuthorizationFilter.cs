using CompanyManagementWeb.Data;
using CompanyManagementWeb.DataAccess;
using CompanyManagementWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagementWeb.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class JwtAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        public ResourceEnum Resource { get; set; }
        public PermissionEnum Permission { get; set; }

        public JwtAuthorizationFilter(ResourceEnum resource, PermissionEnum permission)
        {
            Resource = resource;
            Permission = permission;
        }

        public JwtAuthorizationFilter()
        {
            Resource = ResourceEnum.None;
            Permission = PermissionEnum.None;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
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

            CompanyManagementDbContext dbContext = context.HttpContext.RequestServices.GetRequiredService<CompanyManagementDbContext>();
            int userId = context.HttpContext.Session.GetInt32(SessionVariable.UserId).Value;
            if (Resource != ResourceEnum.None && Permission != PermissionEnum.None)
            {
                int roleId = dbContext.UserCompanies.FirstOrDefault(u => u.UserId == userId).RoleId ?? 0;
                bool isPermitted = dbContext.RolePermissions
                                                .Include(r => r.Resource)
                                                .Include(r => r.Permission)
                                                .Any(r => r.RoleId == roleId 
                                                        && r.Resource.Name.Equals(GetResourceName()) 
                                                        && r.Permission.Name.Equals(GetPermissionName()));

                if (!isPermitted)
                {
                    context.Result = new RedirectToActionResult("Login", "Identity", null);
                    return;
                }
            }

            var departmentId = dbContext.UserCompanies.FirstOrDefault(u => u.UserId == userId).DepartmentId;
            if (departmentId == null)
            {
                context.Result = new RedirectToActionResult("Login", "Identity", null);
                return;
            }

            System.Diagnostics.Debug.WriteLine("Authorize successful", "(AUTHENTICATION)");
        }

        private string? GetResourceName() => ResourceVariable.Get(Resource);
        private string? GetPermissionName() => PermissionVariable.Get(Permission);
    }
}