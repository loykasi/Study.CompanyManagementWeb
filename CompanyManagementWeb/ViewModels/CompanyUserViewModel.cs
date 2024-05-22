using Microsoft.AspNetCore.Mvc.Rendering;

namespace CompanyManagementWeb.ViewModels
{
    public class CompanyUserViewModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }

        public int? RoleId { get; set; }
        public SelectList? Roles { get; set; }

        public int? DepartmentId { get; set; }
        public SelectList? Departments { get; set; }
    }
}