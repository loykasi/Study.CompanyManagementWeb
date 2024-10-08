using CompanyManagementWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CompanyManagementWeb.ViewModels
{
    public class ScheduleViewModel
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? DepartmentName { get; set; }
        public string? EmployeeName { get; set; }

        public string FormattedDate
        {
            get
            {
                if (!Date.HasValue)
                    return "";
                return Date.Value.ToString("dd/MM/yyyy");
            }
        }

        public string FormattedStartTime
        {
            get
            {
                if (!StartTime.HasValue)
                    return "";
                return StartTime.Value.ToString("hh:mm tt");
            }
        }

        public string FormattedEndTime
        {
            get
            {
                if (!EndTime.HasValue)
                    return "";
                return EndTime.Value.ToString("hh:mm tt");
            }
        }
    }
}