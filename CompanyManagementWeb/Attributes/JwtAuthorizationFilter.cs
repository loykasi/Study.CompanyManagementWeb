using CompanyManagementWeb.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CompanyManagementWeb.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class JwtAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string token = context.HttpContext.Session.GetString("Token");
            if (token == null)
            {
                System.Diagnostics.Debug.WriteLine("Redirect", "(LOG)");
                context.Result = new RedirectToActionResult("Login", "Identity", null);
                return;
            }
            IConfiguration configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            if (!JwtHelper.ValidateToken(configuration, token))
            {
                System.Diagnostics.Debug.WriteLine("Redirect", "(LOG)");
                context.Result = new RedirectToActionResult("Login", "Identity", null);
                return;
            }
            System.Diagnostics.Debug.WriteLine("Authorize successful", "(LOG)");
        }
    }
}