using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CompanyManagementWeb.Attributes
{
    public class PhoneNumberAttribute : ValidationAttribute
    {
        private readonly string[] _prefixes = ["090", "098", "091", "031", "035", "038"];
        private readonly string _pattern = @"(090|098|091|031|035|038)+([0-9]{7})\b";

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string? phoneNumber = value as string;
            if (phoneNumber == null)
            {
                return new ValidationResult("Nhập số điện thoại.");
            }

            if (phoneNumber.Length != 10)
            {
                return new ValidationResult("Số điện thoại phải có 10 chữ số");
            }

            if (!Regex.IsMatch(phoneNumber, _pattern))
            {
                return new ValidationResult("Sai định dạng");
            }

            // if (!_prefixes.Any(p => phoneNumber.StartsWith(p)))
            // {
            //     return new ValidationResult("Sai định dạng");
            // }

            return ValidationResult.Success;

        }
    }
}
