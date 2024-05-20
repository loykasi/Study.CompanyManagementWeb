using System.ComponentModel.DataAnnotations;

namespace CompanyManagementWeb.ViewModels
{
    public class CompanyCreateViewModel
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Address { get; set; }
    }
}