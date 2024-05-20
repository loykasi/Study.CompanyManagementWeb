using CompanyManagementWeb.Data;
using CompanyManagementWeb.DataAccess;
using CompanyManagementWeb.Models;
using CompanyManagementWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using CompanyManagementWeb.Attributes;

namespace CompanyManagementWeb.Controllers
{
    public class CompanyUserController : Controller
    {
        private readonly CompanyManagementDbContext _context;
        private readonly ILogger<CompanyUserController> _logger;

        public CompanyUserController(CompanyManagementDbContext context, ILogger<CompanyUserController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [JwtAuthorizationFilter(resource: ResourceEnum.Member, permission: PermissionEnum.View)]
        public async Task<IActionResult> Index()
        {
            CompanyUserIndexViewModel companyUserIndexViewModel = new()
            {
                CompanyUsers = await _context.UserCompanies
                                                .Include(u => u.User)
                                                .Where(u => u.CompanyId == GetCompanyId())
                                                .Select(u => new CompanyUserViewModel
                                                {
                                                    Id = u.UserId,
                                                    Name = u.User.Name,
                                                    DepartmentId = u.DepartmentId,
                                                    RoleId = u.RoleId,
                                                    Departments = new SelectList(
                                                                        _context.Departments.Where(d => d.CompanyId == GetCompanyId()).ToList(),
                                                                        "Id",
                                                                        "Name",
                                                                        u.DepartmentId ?? 0
                                                                    ),
                                                    Roles = new SelectList(
                                                                    _context.Roles.Where(r => r.CompanyId == GetCompanyId()).ToList(),
                                                                    "Id",
                                                                    "Name",
                                                                    u.RoleId ?? 0
                                                                ),
                                                }).ToListAsync()
            };
            return View(companyUserIndexViewModel);
        }

        [JwtAuthorizationFilter(resource: ResourceEnum.Member, permission: PermissionEnum.Edit)]
        [HttpPost]
        public async Task<IActionResult> SetDepartment(int userId, int? departmentId)
        {
            _logger.LogInformation("userId: {userId} | departmentId: {departmentId}", userId, departmentId);
            var userCompany = _context.UserCompanies.FirstOrDefault(u => u.UserId == userId);
            if (userCompany != null)
            {
                userCompany.DepartmentId = departmentId;
                await _context.SaveChangesAsync();
            }
            return Json(Ok());
        }

        [JwtAuthorizationFilter(resource: ResourceEnum.Member, permission: PermissionEnum.Edit)]
        [HttpPost]
        public async Task<IActionResult> SetRole(int userId, int? roleId)
        {
            _logger.LogInformation("userId: {userId} | roleId: {departmentId}", userId, roleId);
            var userCompany = _context.UserCompanies.FirstOrDefault(u => u.UserId == userId);
            if (userCompany != null)
            {
                userCompany.RoleId = roleId;
                await _context.SaveChangesAsync();
            }
            return Json(Ok());
        }

        [JwtAuthorizationFilter(resource: ResourceEnum.Member, permission: PermissionEnum.Edit)]
        [HttpPost]
        public async Task<IActionResult> Delete(int userId)
        {
            _logger.LogInformation("Delete userId: {userId}", userId);
            var userCompany = _context.UserCompanies.FirstOrDefault(u => u.UserId == userId);
            if (userCompany != null)
            {
                _context.UserCompanies.Remove(userCompany);
                await _context.SaveChangesAsync();
            }

            CompanyUserIndexViewModel companyUserIndexViewModel = new()
            {
                CompanyUsers = await _context.UserCompanies
                                                .Include(u => u.User)
                                                .Where(u => u.CompanyId == GetCompanyId())
                                                .Select(u => new CompanyUserViewModel
                                                {
                                                    Id = u.UserId,
                                                    Name = u.User.Name,
                                                    DepartmentId = u.DepartmentId,
                                                    RoleId = u.RoleId,
                                                    Departments = new SelectList(
                                                                        _context.Departments.Where(d => d.CompanyId == GetCompanyId()).ToList(),
                                                                        "Id",
                                                                        "Name",
                                                                        u.DepartmentId ?? 0
                                                                    ),
                                                    Roles = new SelectList(
                                                                    _context.Roles.Where(r => r.CompanyId == GetCompanyId()).ToList(),
                                                                    "Id",
                                                                    "Name",
                                                                    u.RoleId ?? 0
                                                                ),
                                                }).ToListAsync()
            };

            return PartialView("CompanyUsersPartial" ,companyUserIndexViewModel);
        }

        private int GetCompanyId()
        {
            return HttpContext.Session.GetInt32(SessionVariable.CompanyId).Value;
        }

    }
}