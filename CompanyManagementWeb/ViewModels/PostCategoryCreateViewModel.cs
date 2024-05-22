using System.ComponentModel.DataAnnotations;

namespace CompanyManagementWeb.ViewModels
{
    public class PostCategoryCreateViewModel
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }
    }
}