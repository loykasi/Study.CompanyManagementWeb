namespace CompanyManagementWeb.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsAdmin { get; set; } = false;

        public int CompanyId { get; set; }
        public Company? Company { get; set; }
    }
}
