using CompanyManagementWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CompanyManagementWeb.ViewModels
{
    public class ScheduleViewModel
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? DepartmentId { get; set; }

        // public List<Department> Departments { get; set; }
        public SelectList? Departments { get; set; }
    }
}