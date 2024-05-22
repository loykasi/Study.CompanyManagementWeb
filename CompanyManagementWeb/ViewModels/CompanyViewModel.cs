using System.ComponentModel.DataAnnotations;

namespace CompanyManagementWeb.ViewModels
{
    public class CompanyViewModel
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? InviteCode { get; set; }
    }
}