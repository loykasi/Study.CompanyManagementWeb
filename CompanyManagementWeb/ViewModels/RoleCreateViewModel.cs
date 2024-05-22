using System.ComponentModel.DataAnnotations;

namespace CompanyManagementWeb.ViewModels
{
    public class RoleCreateViewModel
    {
        public int? Id { get; set; }
        
        [Required]
        public string? Name { get; set; }
        public bool IsAdmin { get; set; }
        public List<RoleDetailCreateViewModel>? RoleDetails { get; set; }
    }
}