namespace CompanyManagementWeb.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}