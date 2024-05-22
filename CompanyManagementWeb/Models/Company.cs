namespace CompanyManagementWeb.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Code { get; set; }
    }
}
