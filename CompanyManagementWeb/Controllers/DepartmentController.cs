using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompanyManagementWeb.DataAccess;
using CompanyManagementWeb.Models;
using CompanyManagementWeb.ViewModels;
using CompanyManagementWeb.Attributes;

namespace CompanyManagementWeb.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly CompanyManagementDbContext _context;

        public DepartmentController(CompanyManagementDbContext context)
        {
            _context = context;
        }

        // GET: Department
        [JwtAuthorizationFilter]
        public async Task<IActionResult> Index()
        {
            int companyId = HttpContext.Session.GetInt32("companyId").Value;
            var departments = _context.Departments.Include(d => d.Company).Where(d => d.CompanyId == companyId);
            return View(await departments.ToListAsync());
        }

        // GET: Department/Create
        [JwtAuthorizationFilter]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Department/Create
        [HttpPost]
        [JwtAuthorizationFilter]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentCreateViewModel departmentCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                Department department = new()
                {
                    Name = departmentCreateViewModel.Name,
                    CompanyId = HttpContext.Session.GetInt32("companyId").Value
                };
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(departmentCreateViewModel);
        }

        // GET: Department/Edit/5
        [JwtAuthorizationFilter]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            DepartmentCreateViewModel departmentCreateViewModel = new()
            {
                Id = department.Id,
                Name = department.Name
            };
            return View(departmentCreateViewModel);
        }

        // POST: Department/Edit/5
        [HttpPost]
        [JwtAuthorizationFilter]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DepartmentCreateViewModel departmentCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var department = _context.Departments.FirstOrDefault(d => d.Id == departmentCreateViewModel.Id);
                    department.Name = departmentCreateViewModel.Name;
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(departmentCreateViewModel.Id))
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
            return View(departmentCreateViewModel);
        }

        // GET: Department/Delete/5
        [JwtAuthorizationFilter]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [JwtAuthorizationFilter]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }
}
