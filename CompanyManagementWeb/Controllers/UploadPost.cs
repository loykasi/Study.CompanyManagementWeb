using CompanyManagementWeb.DataAccess;
using CompanyManagementWeb.Models;
using CompanyManagementWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
            ViewData["Category"] = new SelectList(_context.PostCategory, "Id", "Name");
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
            PostSearchViewModel postSearchViewModel = new PostSearchViewModel
            {
                Posts = await _context.Posts.Include(p => p.Employee).ToListAsync()
            };
            return View(postSearchViewModel);
        }

        public async Task<IActionResult> Search(PostSearchViewModel postSearchViewModel)
        {
            IQueryable<Post> post = _context.Posts.Include(p => p.Employee);
            if (!postSearchViewModel.SearchValue.IsNullOrEmpty())
            {
                post = post.Where(p => p.Title.Contains(postSearchViewModel.SearchValue));
            }
            if (postSearchViewModel.FromDate != null)
            {
                post = post.Where(p => p.CreatedDate >= postSearchViewModel.FromDate);
            }
            if (postSearchViewModel.ToDate != null)
            {
                post = post.Where(p => p.CreatedDate <= postSearchViewModel.ToDate);
            }
            postSearchViewModel.Posts = await post.ToListAsync();
            return View("ViewPost", postSearchViewModel);
        }
    }
}
