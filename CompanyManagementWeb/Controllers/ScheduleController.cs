using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CompanyManagementWeb.DataAccess;
using CompanyManagementWeb.Models;
using CompanyManagementWeb.ViewModels;
using CompanyManagementWeb.Data;
using CompanyManagementWeb.Attributes;

namespace CompanyManagementWeb.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly CompanyManagementDbContext _context;
        private readonly ILogger<ScheduleController> _logger;

        public ScheduleController(CompanyManagementDbContext context, ILogger<ScheduleController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Schedule
        [JwtAuthorizationFilter(resource: ResourceEnum.Schedule, permission: PermissionEnum.View)]
        public async Task<IActionResult> Index()
        {
            ScheduleIndexViewModel scheduleIndexViewModel = new()
            { 
                Schedules = new List<ScheduleViewModel>()
            };

            var schedules = _context.Schedules.Where(d => d.StartDate.Value.Date >= DateTime.Now.Date && d.CompanyId == GetCompanyId());
            schedules = schedules.Include(s => s.Employee).Include(s => s.Department);

            // var schedules = _context.Schedules.Include(s => s.Employee).Include(s => s.Department).Where(d => d.CompanyId == GetCompanyId());
            foreach (var item in schedules)
            {
                scheduleIndexViewModel.Schedules.Add(new ScheduleViewModel
                    {
                        Id = item.Id,
                        Title = item.Title,
                        Description = item.Description,
                        Content = item.Content,
                        Date = item.StartDate,
                        StartTime = item.StartDate,
                        EndTime = item.EndDate,
                        DepartmentName = item.Department?.Name ?? "",
                        EmployeeName = item.Employee?.Name ?? "",
                    });
            }

            scheduleIndexViewModel.Departments = _context.Departments.Select(d => new SelectListItem
                                                                            {
                                                                                Value = d.Id.ToString(),
                                                                                Text = d.Name
                                                                            });

            return View(scheduleIndexViewModel);
        }

        [JwtAuthorizationFilter(resource: ResourceEnum.Schedule, permission: PermissionEnum.View)]
        public IActionResult Search(ScheduleIndexViewModel scheduleIndexViewModel)
        {
            IQueryable<Schedule> schedules = _context.Schedules.Where(d => d.CompanyId == GetCompanyId());
            bool hasFromDate = false;
            bool hasToDate = false;

            if (scheduleIndexViewModel.DepartmentId != null)
            {
                schedules = schedules.Where(s => s.DepartmentId == scheduleIndexViewModel.DepartmentId);
            }
            if (scheduleIndexViewModel.FromDate != null)
            {
                schedules = schedules.Where(p => p.StartDate >= scheduleIndexViewModel.FromDate);
                hasFromDate = true;
            }
            if (scheduleIndexViewModel.ToDate != null)
            {
                schedules = schedules.Where(p => p.StartDate <= scheduleIndexViewModel.ToDate);
                hasToDate = true;
            }
            if (!hasFromDate && !hasToDate)
            {
                schedules = schedules.Where(p => p.StartDate >= DateTime.Now.Date);
            }
            schedules = schedules.Include(s => s.Employee).Include(s => s.Department);

            _logger.LogInformation("(SCHEDULE SEARCH) {hasFromDate} | {hasToDate}", hasFromDate, hasToDate);

            scheduleIndexViewModel.Schedules = new List<ScheduleViewModel>();
            foreach (var item in schedules)
            {
                scheduleIndexViewModel.Schedules.Add(new ScheduleViewModel
                    {
                        Id = item.Id,
                        Title = item.Title,
                        Description = item.Description,
                        Content = item.Content,
                        Date = item.StartDate,
                        StartTime = item.StartDate,
                        EndTime = item.EndDate,
                        DepartmentName = item.Department?.Name ?? "",
                        EmployeeName = item.Employee?.Name ?? "",
                    });
            }
            return PartialView("ScheduleListPartial", scheduleIndexViewModel);
        }

        [JwtAuthorizationFilter(resource: ResourceEnum.Schedule, permission: PermissionEnum.View)]
        public IActionResult GetOldSchedules()
        {
            ScheduleIndexViewModel scheduleIndexViewModel = new()
            { 
                Schedules = new List<ScheduleViewModel>()
            };

            IQueryable<Schedule> schedules = _context.Schedules.Where(d => d.StartDate.Value.Date < DateTime.Now.Date && d.CompanyId == GetCompanyId()).OrderByDescending(s => s.StartDate.Value.Date);
            schedules = schedules.Include(s => s.Employee).Include(s => s.Department);

            foreach (var item in schedules)
            {
                scheduleIndexViewModel.Schedules.Add(new ScheduleViewModel
                    {
                        Id = item.Id,
                        Title = item.Title,
                        Description = item.Description,
                        Content = item.Content,
                        Date = item.StartDate,
                        StartTime = item.StartDate,
                        EndTime = item.EndDate,
                        DepartmentName = item.Department?.Name ?? "",
                        EmployeeName = item.Employee?.Name ?? "",
                    });
            }
            return PartialView("ScheduleListPartial", scheduleIndexViewModel);
        }

        [JwtAuthorizationFilter(resource: ResourceEnum.Schedule, permission: PermissionEnum.View)]
        public IActionResult GetCurrent()
        {
            ScheduleIndexViewModel scheduleIndexViewModel = new()
            { 
                Schedules = new List<ScheduleViewModel>()
            };

            IQueryable<Schedule> schedules = _context.Schedules.Where(d => d.StartDate.Value.Date >= DateTime.Now.Date && d.CompanyId == GetCompanyId()).OrderBy(s => s.StartDate.Value.Date);
            schedules = schedules.Include(s => s.Employee).Include(s => s.Department);

            foreach (var item in schedules)
            {
                scheduleIndexViewModel.Schedules.Add(new ScheduleViewModel
                    {
                        Id = item.Id,
                        Title = item.Title,
                        Description = item.Description,
                        Content = item.Content,
                        Date = item.StartDate,
                        StartTime = item.StartDate,
                        EndTime = item.EndDate,
                        DepartmentName = item.Department?.Name ?? "",
                        EmployeeName = item.Employee?.Name ?? "",
                    });
            }
            return PartialView("ScheduleListPartial", scheduleIndexViewModel);
        }

        // DETAIL: Schedule/Detail/5
        [JwtAuthorizationFilter(resource: ResourceEnum.Schedule, permission: PermissionEnum.View)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules.Include(s => s.Department).Include(s => s.Employee).FirstOrDefaultAsync(s => s.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }
            
            ScheduleViewModel scheduleViewModel = new()
            {
                Id = schedule.Id,
                Title = schedule.Title,
                Description = schedule.Description,
                Content = schedule.Content,
                Date = schedule.StartDate,
                StartTime = schedule.StartDate,
                EndTime = schedule.EndDate,
                DepartmentName = schedule.Department?.Name ?? "",
                EmployeeName = schedule.Employee?.Name ?? "",
            };
            return View(scheduleViewModel);
        }

        // GET: Schedule/Create
        [JwtAuthorizationFilter(resource: ResourceEnum.Schedule, permission: PermissionEnum.Edit)]
        public IActionResult Create()
        {
            ScheduleEditViewModel scheduleViewModel = new()
            {
                Departments = new SelectList(_context.Departments.Where(d => d.CompanyId == GetCompanyId()), "Id", "Name")
            };
            return View(scheduleViewModel);
        }

        // POST: Schedule/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [JwtAuthorizationFilter(resource: ResourceEnum.Schedule, permission: PermissionEnum.Edit)]
        public async Task<IActionResult> Create(ScheduleEditViewModel scheduleViewModel)
        {   
            if (ModelState.IsValid)
            {
                if (scheduleViewModel.EndTime.Value < scheduleViewModel.StartTime.Value)
                {
                    ModelState.AddModelError("DateError", "Thời gian kết thúc phải sau thời gian bắt đầu");
                    scheduleViewModel.Departments = new SelectList(_context.Departments.Where(d => d.CompanyId == GetCompanyId()), "Id", "Name");
                    return View(scheduleViewModel);
                }
                // To do: get current user ID
                int employeeId = GetUserId();

                Schedule schedule = new()
                {
                    Title = scheduleViewModel.Title,
                    Description = scheduleViewModel.Description,
                    Content = scheduleViewModel.Content,
                    StartDate = new DateTime(scheduleViewModel.Date.Value.Year, scheduleViewModel.Date.Value.Month, scheduleViewModel.Date.Value.Day,
                                            scheduleViewModel.StartTime.Value.Hour, scheduleViewModel.StartTime.Value.Minute, scheduleViewModel.StartTime.Value.Second),
                    EndDate = new DateTime(scheduleViewModel.Date.Value.Year, scheduleViewModel.Date.Value.Month, scheduleViewModel.Date.Value.Day,
                                            scheduleViewModel.EndTime.Value.Hour, scheduleViewModel.EndTime.Value.Minute, scheduleViewModel.EndTime.Value.Second),
                    EmployeeId = employeeId,
                    DepartmentId = scheduleViewModel.DepartmentId,
                    CompanyId = GetCompanyId()
                };

                _context.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(scheduleViewModel);
        }

        // GET: Schedule/Edit/5
        [JwtAuthorizationFilter(resource: ResourceEnum.Schedule, permission: PermissionEnum.Edit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            
            ScheduleEditViewModel scheduleViewModel = new()
            {
                Id = schedule.Id,
                Title = schedule.Title,
                Description = schedule.Description,
                Content = schedule.Content,
                Date = schedule.StartDate,
                StartTime = schedule.StartDate,
                EndTime = schedule.EndDate,
                Departments = new SelectList(_context.Departments, "Id", "Name", schedule.DepartmentId)
            };
            return View(scheduleViewModel);
        }

        // POST: Schedule/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [JwtAuthorizationFilter(resource: ResourceEnum.Schedule, permission: PermissionEnum.Edit)]
        public async Task<IActionResult> Edit(ScheduleEditViewModel scheduleViewModel)
        {
            if (ModelState.IsValid)
            {
                if (scheduleViewModel.EndTime.Value < scheduleViewModel.StartTime.Value)
                {
                    ModelState.AddModelError("DateError", "Thời gian kết thúc phải sau thời gian bắt đầu");
                    scheduleViewModel.Departments = new SelectList(_context.Departments.Where(d => d.CompanyId == GetCompanyId()), "Id", "Name");
                    return View(scheduleViewModel);
                }

                try
                {
                    var schedule = await _context.Schedules.FindAsync(scheduleViewModel.Id);
                    if (schedule == null)
                    {
                        return NotFound();
                    }

                    schedule.Title = scheduleViewModel.Title;
                    schedule.Description = scheduleViewModel.Description;
                    schedule.Content = scheduleViewModel.Content;
                    schedule.StartDate = new DateTime(scheduleViewModel.Date.Value.Year, scheduleViewModel.Date.Value.Month, scheduleViewModel.Date.Value.Day,
                                            scheduleViewModel.StartTime.Value.Hour, scheduleViewModel.StartTime.Value.Minute, scheduleViewModel.StartTime.Value.Second);
                    schedule.EndDate = new DateTime(scheduleViewModel.Date.Value.Year, scheduleViewModel.Date.Value.Month, scheduleViewModel.Date.Value.Day,
                                            scheduleViewModel.EndTime.Value.Hour, scheduleViewModel.EndTime.Value.Minute, scheduleViewModel.EndTime.Value.Second);
                    schedule.EmployeeId = 1;
                    schedule.DepartmentId = scheduleViewModel.DepartmentId;
                    
                    _context.Update(schedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(scheduleViewModel.Id.Value))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            
            return View(scheduleViewModel);
        }

        // GET: Schedule/Delete/5
        [JwtAuthorizationFilter(resource: ResourceEnum.Schedule, permission: PermissionEnum.Edit)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            
            ScheduleEditViewModel scheduleViewModel = new()
            {
                Id = schedule.Id,
                Title = schedule.Title,
                Description = schedule.Description,
                Date = schedule.StartDate,
                StartTime = schedule.StartDate,
                EndTime = schedule.EndDate,
                Departments = new SelectList(_context.Departments, "Id", "Name", schedule.DepartmentId)
            };

            return View(scheduleViewModel);
        }

        // POST: Schedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [JwtAuthorizationFilter(resource: ResourceEnum.Schedule, permission: PermissionEnum.Edit)]
        public async Task<IActionResult> DeleteConfirmed(ScheduleEditViewModel scheduleViewModel)
        {
            var schedule = await _context.Schedules.FindAsync(scheduleViewModel.Id);
            if (schedule != null)
            {
                _context.Schedules.Remove(schedule);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(int id)
        {
            return _context.Schedules.Any(e => e.Id == id);
        }

        private int GetCompanyId()
        {
            return HttpContext.Session.GetInt32(SessionVariable.CompanyId).Value;
        }

        private int GetUserId()
        {
            return HttpContext.Session.GetInt32(SessionVariable.UserId).Value;
        }
    }
}
