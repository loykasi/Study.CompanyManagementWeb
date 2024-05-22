using System.ComponentModel.DataAnnotations;

namespace CompanyManagementWeb.ViewModels
{
    public class DepartmentCreateViewModel
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }
    }
}