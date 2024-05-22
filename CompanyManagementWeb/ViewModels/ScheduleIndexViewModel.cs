using Microsoft.AspNetCore.Mvc.Rendering;

namespace CompanyManagementWeb.ViewModels
{
    public class ScheduleIndexViewModel
    {
        public List<ScheduleViewModel> Schedules { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }

        public int? DepartmentId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}