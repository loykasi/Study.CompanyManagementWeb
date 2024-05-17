namespace CompanyManagementWeb.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }

        public string? RefreshToken { get; set; }
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}
