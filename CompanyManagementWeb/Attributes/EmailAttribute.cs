using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CompanyManagementWeb.Attributes
{
    public class EmailAttribute : ValidationAttribute
    {
        private readonly string _pattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string? email = value as string;
            if (email == null)
            {
                return new ValidationResult("Nhập email.");
            }

            if (!Regex.IsMatch(email, _pattern))
            {
                return new ValidationResult("Sai định dạng.");
            }

            return ValidationResult.Success;

        }
    }
}
