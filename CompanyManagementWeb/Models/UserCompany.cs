namespace CompanyManagementWeb.Models
{
    public class UserCompany
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
        
        public int CompanyId { get; set; }
        public Company? Company { get; set; }

        public int? RoleId { get; set; }
        public Role? Role { get; set; }

        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}
