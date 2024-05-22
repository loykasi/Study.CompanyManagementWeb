using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompanyManagementWeb.DataAccess;
using CompanyManagementWeb.Models;
using CompanyManagementWeb.ViewModels;
using CompanyManagementWeb.Attributes;
using CompanyManagementWeb.Data;
using Microsoft.AspNetCore.Http.HttpResults;

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
        [JwtAuthorizationFilter(resource: ResourceEnum.Department, permission: PermissionEnum.View)]
        public async Task<IActionResult> Index()
        {
            int companyId = HttpContext.Session.GetInt32("companyId").Value;
            var departments = _context.Departments.Include(d => d.Company).Where(d => d.CompanyId == companyId);
            DepartmentViewModel departmentViewModel = new()
            {
                Departments = await departments.ToListAsync()
            };
            return View(departmentViewModel);
        }

        [HttpPost]
        [JwtAuthorizationFilter(resource: ResourceEnum.Department, permission: PermissionEnum.Edit)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentViewModel departmentViewModel)
        {
            if (ModelState.IsValid)
            {
                Department department = new()
                {
                    Name = departmentViewModel.DepartmentCreateViewModel.Name,
                    CompanyId = HttpContext.Session.GetInt32("companyId").Value
                };
                _context.Add(department);
                await _context.SaveChangesAsync();

                int companyId = HttpContext.Session.GetInt32("companyId").Value;
                var departments = _context.Departments.Include(d => d.Company).Where(d => d.CompanyId == companyId);
                departmentViewModel.Departments = await departments.ToListAsync();
                return PartialView("DepartmentListPartial", departmentViewModel);
            }

            return NoContent();
        }

        // POST: Department/Edit/5
        [HttpPost]
        [JwtAuthorizationFilter(resource: ResourceEnum.Department, permission: PermissionEnum.Edit)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DepartmentViewModel departmentViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var department = _context.Departments.FirstOrDefault(d => d.Id == departmentViewModel.DepartmentCreateViewModel.Id);
                    department.Name = departmentViewModel.DepartmentCreateViewModel.Name;
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(departmentViewModel.DepartmentCreateViewModel.Id))
                    {
                        return NoContent();
                    }
                    else
                    {
                        throw;
                    }
                }
                
                int companyId = HttpContext.Session.GetInt32("companyId").Value;
                var departments = _context.Departments.Include(d => d.Company).Where(d => d.CompanyId == companyId);
                departmentViewModel.Departments = await departments.ToListAsync();
                return PartialView("DepartmentListPartial", departmentViewModel);
            }
            return NoContent();
        }

        // POST: Department/Delete/5
        [HttpPost]
        [JwtAuthorizationFilter(resource: ResourceEnum.Department, permission: PermissionEnum.Edit)]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
            }

            await _context.SaveChangesAsync();

            int companyId = HttpContext.Session.GetInt32("companyId").Value;
            var departments = _context.Departments.Include(d => d.Company).Where(d => d.CompanyId == companyId);
            DepartmentViewModel departmentViewModel = new()
            {
                Departments = await departments.ToListAsync()
            };
            return PartialView("DepartmentListPartial", departmentViewModel);
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }
}
