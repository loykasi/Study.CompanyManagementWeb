using Microsoft.AspNetCore.Mvc.Rendering;

namespace CompanyManagementWeb.ViewModels
{
    public class PostCreateViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public int? CategoryID { get; set; }
        public int? DepartmentId { get; set; }

        public SelectList? Categories { get; set; }
        public SelectList? Departments { get; set; }
    }
}