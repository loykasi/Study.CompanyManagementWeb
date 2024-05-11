using CompanyManagementWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CompanyManagementWeb.ViewModels
{
    public class ScheduleIndexViewModel
    {
        public List<ScheduleViewModel> Schedules { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }

        public int? DepartmentId { get; set; }
    }
}