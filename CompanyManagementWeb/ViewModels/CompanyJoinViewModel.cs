using System.ComponentModel.DataAnnotations;

namespace CompanyManagementWeb.ViewModels
{
    public class CompanyJoinViewModel
    {
        [Required]
        public string? Code { get; set; }
    }
}