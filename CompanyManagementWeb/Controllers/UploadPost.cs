using CompanyManagementWeb.DataAccess;
using CompanyManagementWeb.Models;
using CompanyManagementWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagementWeb.Controllers
{
    public class UploadPost : Controller
    {
        private readonly CompanyManagementDbContext _context;
        
        public UploadPost(CompanyManagementDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content")] PostViewModel postViewModel)
        {
            if (ModelState.IsValid)
            {
                // To do: get current user ID
                int employeeId = 1;

                Post post = new Post
                {
                    Title = postViewModel.Title,
                    Content = postViewModel.Content,
                    CreatedDate = DateTime.Now,
                    EmployeeId = employeeId
                };

                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View("Index", postViewModel);
        }

        public async Task<IActionResult> ViewPost()
        {
            var companyManagementDbContext = _context.Posts.Include(p => p.Employee);
            return View(await companyManagementDbContext.ToListAsync());
        }
    }
}
