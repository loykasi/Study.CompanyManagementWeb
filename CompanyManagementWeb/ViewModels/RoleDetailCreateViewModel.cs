using Microsoft.AspNetCore.Mvc.Rendering;

namespace CompanyManagementWeb.ViewModels
{
    public class RoleDetailCreateViewModel
    {
        public int ResourceId { get; set; }
        public string? Resource { get; set; }
        public int PermissionId { get; set; }
        public SelectList? Permissions { get; set; }
    }
}