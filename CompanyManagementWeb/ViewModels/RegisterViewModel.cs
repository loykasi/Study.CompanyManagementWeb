using System.ComponentModel.DataAnnotations;
using CompanyManagementWeb.Attributes;

namespace CompanyManagementWeb.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        [Email]
        public string? Email { get; set; }

        [Required]
        [PhoneNumber]
        public string? PhoneNumber { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        public string? ConfirmedPassword { get; set; }
    }
}