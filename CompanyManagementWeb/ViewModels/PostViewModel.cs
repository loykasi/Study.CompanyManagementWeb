namespace CompanyManagementWeb.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CategoryName { get; set; }
        public string? DepartmentName { get; set; }
        public string? EmployeeName { get; set; }
    }
}