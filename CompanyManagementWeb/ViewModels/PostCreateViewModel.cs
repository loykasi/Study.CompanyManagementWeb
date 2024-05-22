using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CompanyManagementWeb.ViewModels
{
    public class PostCreateViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 20)]
        public string? Title { get; set; }

        [Required]
        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public string? Content { get; set; }
        
        public int? CategoryID { get; set; }
        public int? DepartmentId { get; set; }

        public SelectList? Categories { get; set; }
        public SelectList? Departments { get; set; }
    }
}