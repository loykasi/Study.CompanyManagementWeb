using Microsoft.AspNetCore.Mvc.Rendering;

namespace CompanyManagementWeb.ViewModels
{
    public class PostIndexViewModel
    {
        public List<PostViewModel> Posts { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }

        public string? SearchValue { get; set; }
        public int? DepartmentId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}