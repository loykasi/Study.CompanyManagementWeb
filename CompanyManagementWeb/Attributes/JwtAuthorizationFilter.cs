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
                RedirectToLogin(context);
                return;
            }

            int? userId = context.HttpContext.Session.GetInt32("userId");
            if (!userId.HasValue)
            {
                RedirectToLogin(context);
                return;
            }

            if (!IsAccessTokenValid(context, token, userId.Value))
            {
                RedirectToLogin(context);
                return;
            }

            if (!IsPermitted(context, userId.Value))
            {
                RedirectToLogin(context);
                return;
            }
        }

        private bool IsAccessTokenValid(AuthorizationFilterContext context, string token, int userId)
        {
            ITokenService tokenService = context.HttpContext.RequestServices.GetRequiredService<ITokenService>();
            if (!tokenService.ValidateAccessToken(token))
            {
                string refreshToken = context.HttpContext.Request.Cookies["refreshTokenCookie"]!;
                if(!tokenService.IsRefreshTokenValid(refreshToken).Result)
                {
                    return false;   
                }
                var tokens = tokenService.GenerateToken(new Models.User { Id = userId });
                tokenService.SetJWTCookie(context.HttpContext, tokens.AccessToken!);
                tokenService.SetRefreshTokenCookie(context.HttpContext, userId, tokens.RefreshToken!);
            }
            return true;
        }

        private bool IsPermitted(AuthorizationFilterContext context, int userId)
        {
            CompanyManagementDbContext dbContext = context.HttpContext.RequestServices.GetRequiredService<CompanyManagementDbContext>();
            bool isAdmin = false;
            if (Resource != ResourceEnum.None && Permission != PermissionEnum.None)
            {
                int roleId = dbContext.UserCompanies.FirstOrDefault(u => u.UserId == userId).RoleId ?? 0;

                var role = dbContext.Roles.Find(roleId);
                bool isPermitted;

                if (role != null && role.IsAdmin)
                {
                    isPermitted = true;
                    isAdmin = true;
                }
                else
                {
                    var userPermission = dbContext.RolePermissions
                                                .Include(r => r.Resource)
                                                .Include(r => r.Permission)
                                                .FirstOrDefault(r => r.RoleId == roleId && r.Resource.Name.Equals(GetResourceName()));
                    isPermitted = IsInPermission(userPermission.Permission.Name);
                }

                if (!isPermitted)
                {
                    return false;
                }
            }

            var departmentId = dbContext.UserCompanies.FirstOrDefault(u => u.UserId == userId).DepartmentId;
            if (!isAdmin && departmentId == null)
            {
                return false;
            }

            return true;
        }

        private void RedirectToLogin(AuthorizationFilterContext context)
        {
            context.Result = new RedirectToActionResult("Login", "Identity", null);
        }

        private bool IsInPermission(string userPermission)
        {
            return userPermission switch
            {
                null => false,
                "View" => userPermission.Equals(GetPermissionName()),
                "Edit" => userPermission.Equals(GetPermissionName()) || Permission == PermissionEnum.View,
                _ => false,
            };
        }

        private string? GetResourceName() => ResourceVariable.Get(Resource);
        private string? GetPermissionName() => PermissionVariable.Get(Permission);
    }
}