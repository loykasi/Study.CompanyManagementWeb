namespace CompanyManagementWeb.ViewModels
{
    public class RoleCreateViewModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public List<RoleDetailCreateViewModel>? RoleDetails { get; set; }
    }
}