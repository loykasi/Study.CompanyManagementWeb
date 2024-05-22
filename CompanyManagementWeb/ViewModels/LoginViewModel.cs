using System.ComponentModel.DataAnnotations;
using CompanyManagementWeb.Attributes;

namespace CompanyManagementWeb.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Sai địa chỉ Email")]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}