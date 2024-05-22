using System.ComponentModel.DataAnnotations;
using CompanyManagementWeb.Models;

namespace CompanyManagementWeb.ViewModels
{
    public class DepartmentViewModel
    {
        public List<Department>? Departments { get; set; }
        public DepartmentCreateViewModel DepartmentCreateViewModel { get; set; }
    }
}