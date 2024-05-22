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
            _dbContext.Resources.Add(new Resource { Name = ResourceVariable.Get(ResourceEnum.Post) });
            _dbContext.Resources.Add(new Resource { Name = ResourceVariable.Get(ResourceEnum.Schedule) });
            _dbContext.Resources.Add(new Resource { Name = ResourceVariable.Get(ResourceEnum.PostCategory) });
            _dbContext.Resources.Add(new Resource { Name = ResourceVariable.Get(ResourceEnum.Department) });
            _dbContext.Resources.Add(new Resource { Name = ResourceVariable.Get(ResourceEnum.Role) });
            _dbContext.Resources.Add(new Resource { Name = ResourceVariable.Get(ResourceEnum.Member) });

            _dbContext.Permissions.Add(new Permission { Name = PermissionVariable.Get(PermissionEnum.None) });
            _dbContext.Permissions.Add(new Permission { Name = PermissionVariable.Get(PermissionEnum.Edit) });
            _dbContext.Permissions.Add(new Permission { Name = PermissionVariable.Get(PermissionEnum.View) });

            // var pers = await _dbContext.Permissions.ToListAsync();
            // foreach (var item in pers)
            // {
            //     _dbContext.Permissions.Remove(item);
            // }

            // var res = await _dbContext.Resources.ToListAsync();
            // foreach (var item in res)
            // {
            //     _dbContext.Resources.Remove(item);
            // }
            
            
            await _dbContext.SaveChangesAsync();

            StatusMessage = "Vừa seed Database";
            return RedirectToAction("Index");
        }
    }
}
