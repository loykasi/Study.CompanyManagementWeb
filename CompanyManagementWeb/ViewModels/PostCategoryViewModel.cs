using System.ComponentModel.DataAnnotations;
using CompanyManagementWeb.Models;

namespace CompanyManagementWeb.ViewModels
{
    public class PostCategoryViewModel
    {
        public List<PostCategory>? PostCategories { get; set; }
        public PostCategoryCreateViewModel PostCategoryCreateViewModel { get; set; }
    }
}