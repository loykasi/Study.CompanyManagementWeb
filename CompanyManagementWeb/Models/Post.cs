namespace CompanyManagementWeb.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int PostCategoryId { get; set; }
        public PostCategory? PostCategory { get; set; }

        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}