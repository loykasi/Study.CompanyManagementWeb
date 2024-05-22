namespace CompanyManagementWeb.ViewModels
{
    public class RoleViewModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public bool IsAdmin { get; set; }
        public IEnumerable<RoleDetailViewModel>? RoleDetails { get; set; }
    }
}