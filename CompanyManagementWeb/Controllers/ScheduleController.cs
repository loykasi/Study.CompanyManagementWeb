using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CompanyManagementWeb.DataAccess;
using CompanyManagementWeb.Models;
using CompanyManagementWeb.ViewModels;

namespace CompanyManagementWeb.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly CompanyManagementDbContext _context;

        public ScheduleController(CompanyManagementDbContext context)
        {
            _context = context;
        }

        // GET: Schedule
        public async Task<IActionResult> Index()
        {
            var schedules = _context.Schedules.Include(s => s.Employee).Include(s => s.Department);
            return View(await schedules.ToListAsync());
        }

        // GET: Schedule/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // GET: Schedule/Create
        public IActionResult Create()
        {
            ScheduleViewModel scheduleViewModel = new()
            {
                Departments = new SelectList(_context.Departments, "Id", "Name")
            };
            return View(scheduleViewModel);
        }

        // POST: Schedule/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ScheduleViewModel scheduleViewModel)
        {
            System.Diagnostics.Debug.WriteLine("LOG: " + scheduleViewModel.Date.Value);
            System.Diagnostics.Debug.WriteLine("LOG: " + scheduleViewModel.StartTime.Value);
            System.Diagnostics.Debug.WriteLine("LOG: " + scheduleViewModel.EndTime.Value);
            
            if (ModelState.IsValid)
            {
                // To do: get current user ID
                int employeeId = 1;

                Schedule schedule = new()
                {
                    Title = scheduleViewModel.Title,
                    Description = scheduleViewModel.Description,
                    StartDate = new DateTime(scheduleViewModel.Date.Value.Year, scheduleViewModel.Date.Value.Month, scheduleViewModel.Date.Value.Day,
                                            scheduleViewModel.StartTime.Value.Hour, scheduleViewModel.StartTime.Value.Minute, scheduleViewModel.StartTime.Value.Second),
                    EndDate = new DateTime(scheduleViewModel.Date.Value.Year, scheduleViewModel.Date.Value.Month, scheduleViewModel.Date.Value.Day,
                                            scheduleViewModel.EndTime.Value.Hour, scheduleViewModel.EndTime.Value.Minute, scheduleViewModel.EndTime.Value.Second),
                    EmployeeId = employeeId,
                    DepartmentId = scheduleViewModel.DepartmentId
                };

                _context.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(scheduleViewModel);
        }

        // GET: Schedule/Edit/5
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
            
            ScheduleViewModel scheduleViewModel = new()
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

        // POST: Schedule/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ScheduleViewModel scheduleViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var schedule = await _context.Schedules.FindAsync(scheduleViewModel.Id);
                    if (schedule == null)
                    {
                        return NotFound();
                    }

                    schedule.Title = scheduleViewModel.Title;
                    schedule.Description = scheduleViewModel.Description;
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
            
            ScheduleViewModel scheduleViewModel = new()
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
        public async Task<IActionResult> DeleteConfirmed(ScheduleViewModel scheduleViewModel)
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
    }
}
