using System.ComponentModel.DataAnnotations;
using CompanyManagementWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CompanyManagementWeb.ViewModels
{
    public class ScheduleEditViewModel
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 20)]
        public string? Title { get; set; }

        [Required]
        public string? Description { get; set; }

        public string? Content { get; set; }

        [Required]
        public DateTime? Date { get; set; }

        [Required]
        public DateTime? StartTime { get; set; }

        [Required]
        public DateTime? EndTime { get; set; }
        public int? DepartmentId { get; set; }

        // public List<Department> Departments { get; set; }
        public SelectList? Departments { get; set; }

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

        public string SelectedDepartment
        {
            get
            {
                if (Departments == null)
                    return "";
                var department = Departments.FirstOrDefault(d => d.Selected);
                if (department == null)
                    return "";
                return department.Text;
            }
        }
    }
}