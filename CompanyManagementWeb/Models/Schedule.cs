namespace CompanyManagementWeb.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        public int CompanyId { get; set; }
        public Company? Company { get; set; }

        public int EmployeeId { get; set; }
        public User? Employee { get; set; }
    }
}
