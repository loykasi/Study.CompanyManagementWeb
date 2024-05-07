using CompanyManagementWeb.Models;

namespace CompanyManagementWeb.ViewModels
{
    public class PostSearchViewModel
    {
        public string? SearchValue { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public List<Post>? Posts { get; set; }
    }
}