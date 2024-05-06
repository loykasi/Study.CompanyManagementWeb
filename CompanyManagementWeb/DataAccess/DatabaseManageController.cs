using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagementWeb.DataAccess
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
    }
}
