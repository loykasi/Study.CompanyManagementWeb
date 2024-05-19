using CompanyManagementWeb.Data;
using CompanyManagementWeb.DataAccess;
using CompanyManagementWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagementWeb.Controllers
{
    public class DatabaseManageController : Controller
    {
        private readonly CompanyManagementDbContext _dbContext;

        [TempData]
        public string StatusMessage { get; set; }

        public DatabaseManageController(CompanyManagementDbContext dbContext)
        {
            _dbContext = dbContext;
            StatusMessage = string.Empty;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Delete()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteAsync()
        {
            var success = await _dbContext.Database.EnsureDeletedAsync();
            StatusMessage = success ? "Xóa Database thành công" : "Không xóa được ";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Migrate()
        {
            await _dbContext.Database.MigrateAsync();
            StatusMessage = "Cập nhật Database thành công";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> SeedDataAsync()
        {
            //List<Department> departments =
            //[
            //    new Department { Name = "Product Development"},
            //    new Department { Name = "Product Management"},
            //    new Department { Name = "Sale and Marketing"},
            //    new Department { Name = "Customer Support"},
            //    new Department { Name = "Human Resources"},
            //    new Department { Name = "IT Services"},
            //    new Department { Name = "Quality Assurance"},
            //];

            //_dbContext.Departments.Add(departments[0]);
            //_dbContext.Departments.Add(departments[1]);
            //_dbContext.Departments.Add(departments[2]);
            //_dbContext.Departments.Add(departments[3]);
            //_dbContext.Departments.Add(departments[4]);
            //_dbContext.Departments.Add(departments[5]);
            //_dbContext.Departments.Add(departments[6]);
            //await _dbContext.SaveChangesAsync();

            //_dbContext.Users.Add(new User { Name = "Lam An", DepartmentId = departments[0].Id });
            //_dbContext.Users.Add(new User { Name = "Khanh Duy", DepartmentId = departments[1].Id });
            //_dbContext.Users.Add(new User { Name = "Ngoc Tin", DepartmentId = departments[2].Id });
            //_dbContext.Users.Add(new User { Name = "Thu Thuy", DepartmentId = departments[3].Id });

            _dbContext.Resources.Add(new Resource { Name = ResourceVariable.Get(ResourceEnum.Post) });
            _dbContext.Resources.Add(new Resource { Name = ResourceVariable.Get(ResourceEnum.Schedule) });

            _dbContext.Permissions.Add(new Permission { Name = PermissionVariable.Get(PermissionEnum.Edit) });
            _dbContext.Permissions.Add(new Permission { Name = PermissionVariable.Get(PermissionEnum.View) });
            await _dbContext.SaveChangesAsync();

            StatusMessage = "Vừa seed Database";
            return RedirectToAction("Index");
        }
    }
}
