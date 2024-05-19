using CompanyManagementWeb.Attributes;
using CompanyManagementWeb.Data;
using CompanyManagementWeb.DataAccess;
using CompanyManagementWeb.Models;
using CompanyManagementWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CompanyManagementWeb.Controllers
{
    public class PostController : Controller
    {
        private readonly CompanyManagementDbContext _context;

        public PostController(CompanyManagementDbContext context)
        {
            _context = context;
        }

        // GET: UploadPost
        [JwtAuthorizationFilter(resource: ResourceEnum.Post, permission: PermissionEnum.Edit)]
        public async Task<IActionResult> Index()
        {
            PostIndexViewModel postIndexViewModel = new()
            { 
                Posts = new List<PostViewModel>()
            };

            
            var schedules = _context.Posts.Include(s => s.PostCategory)
                                            .Include(s => s.Employee)
                                            .Include(s => s.Department)
                                            .Where(d => d.PostCategory.CompanyId == GetCompanyId());;
            foreach (var item in schedules)
            {
                postIndexViewModel.Posts.Add(new PostViewModel
                    {
                        Id = item.Id,
                        Title = item.Title,
                        Description = item.Description,
                        Content = item.Content,
                        CreatedDate = item.CreatedDate,
                        CategoryName = item.PostCategory?.Name ?? "",
                        DepartmentName = item.Department?.Name ?? "",
                        EmployeeName = item.Employee?.Name ?? "",
                    });
            }

            postIndexViewModel.Departments = _context.Departments.Select(d => new SelectListItem
                                                                            {
                                                                                Value = d.Id.ToString(),
                                                                                Text = d.Name
                                                                            });
            return View(postIndexViewModel);
        }

        [JwtAuthorizationFilter]
        public async Task<IActionResult> Search(PostIndexViewModel postIndexViewModel)
        {
            IQueryable<Post> posts = _context.Posts;

            if (!postIndexViewModel.SearchValue.IsNullOrEmpty())
            {
                posts = posts.Where(p => p.Title.Contains(postIndexViewModel.SearchValue));
            }
            if (postIndexViewModel.DepartmentId != null)
            {
                posts = posts.Where(p => p.DepartmentId == postIndexViewModel.DepartmentId);
            }
            if (postIndexViewModel.FromDate != null)
            {
                posts = posts.Where(p => p.CreatedDate >= postIndexViewModel.FromDate);
            }
            if (postIndexViewModel.ToDate != null)
            {
                posts = posts.Where(p => p.CreatedDate <= postIndexViewModel.ToDate);
            }
            posts = posts.Include(s => s.PostCategory).Include(s => s.Employee).Include(s => s.Department);

            postIndexViewModel.Posts = new List<PostViewModel>();
            foreach (var item in posts)
            {
                postIndexViewModel.Posts.Add(new PostViewModel
                    {
                        Id = item.Id,
                        Title = item.Title,
                        Description = item.Description,
                        Content = item.Content,
                        CreatedDate = item.CreatedDate,
                        CategoryName = item.PostCategory?.Name ?? "",
                        DepartmentName = item.Department?.Name ?? "",
                        EmployeeName = item.Employee?.Name ?? "",
                    });
            }

            postIndexViewModel.Departments = _context.Departments.Select(d => new SelectListItem
                                                                            {
                                                                                Value = d.Id.ToString(),
                                                                                Text = d.Name
                                                                            });
            return View("Index", postIndexViewModel);
        }

        // GET: UploadPost/Details/5
        [JwtAuthorizationFilter]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                            .Include(p => p.PostCategory)
                            .Include(p => p.Employee)
                            .Include(p => p.Department)
                            .FirstOrDefaultAsync(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            PostViewModel postViewModel = new()
            {
                Id = post.Id,
                Title = post.Title,
                Description = post.Description,
                Content = post.Content,
                CreatedDate = post.CreatedDate,
                CategoryName = post.PostCategory?.Name ?? "",
                DepartmentName = post.Department?.Name ?? "",
                EmployeeName = post.Employee?.Name ?? "",
            };

            return View(postViewModel);
        }

        // GET: UploadPost/Create
        [JwtAuthorizationFilter]
        public IActionResult Create()
        {
            PostCreateViewModel postCreateViewModel = new()
            {
                Categories = new SelectList(_context.PostCategories.Where(p => p.CompanyId == GetCompanyId()), "Id", "Name"),
                Departments = new SelectList(_context.Departments.Where(d => d.CompanyId == GetCompanyId()), "Id", "Name")
            };
            return View(postCreateViewModel);
        }

        // POST: UploadPost/Create
        [HttpPost]
        [JwtAuthorizationFilter]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostCreateViewModel postViewModel)
        {
            if (ModelState.IsValid)
            {
                // To do: get current user ID
                int employeeId = 1;

                Post post = new Post
                {
                    Title = postViewModel.Title,
                    Description = postViewModel.Description,
                    Content = postViewModel.Content,
                    CreatedDate = DateTime.Now,
                    PostCategoryId = postViewModel.CategoryID.Value,
                    DepartmentId = postViewModel.DepartmentId,
                    EmployeeId = employeeId,
                    // CompanyId = HttpContext.Session.GetInt32(SessionVariable.CompanyId).Value
                };

                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View("Index", postViewModel);
        }

        // GET: UploadPost/Edit/5
        [JwtAuthorizationFilter]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            
            PostCreateViewModel postCreateViewModel = new()
            {
                Id = post.Id,
                Title = post.Title,
                Description = post.Description,
                Content = post.Content,
                CategoryID = post.PostCategoryId,
                DepartmentId = post.DepartmentId,
                Categories = new SelectList(_context.PostCategories, "Id", "Name", post.PostCategoryId),
                Departments = new SelectList(_context.Departments, "Id", "Name", post.DepartmentId)
            };
            
            return View(postCreateViewModel);
        }

        // POST: UploadPost/Edit/5
        [HttpPost]
        [JwtAuthorizationFilter]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PostCreateViewModel postViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var post = await _context.Posts.FindAsync(postViewModel.Id);
                    if (post == null)
                    {
                        return NotFound();
                    }
                    post.Title = postViewModel.Title;
                    post.Content = postViewModel.Content;
                    post.Description = postViewModel.Description;
                    post.PostCategoryId = postViewModel.CategoryID.Value;
                    post.DepartmentId = postViewModel.DepartmentId;

                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(postViewModel.Id))
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
            
            return View(postViewModel);
        }

        // GET: UploadPost/Delete/5
        [JwtAuthorizationFilter]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                                        .Include(p => p.PostCategory)
                                        .Include(p => p.Employee)
                                        .Include(p => p.Department)
                                        .FirstOrDefaultAsync(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            PostViewModel postViewModel = new PostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Description = post.Description,
                Content = post.Content,
                CreatedDate = post.CreatedDate,
                CategoryName = post.PostCategory?.Name ?? "",
                DepartmentName = post.Department?.Name ?? "",
                EmployeeName = post.Employee?.Name ?? "",
            };

            return View(postViewModel);
        }

        // POST: UploadPost/Delete/5
        [HttpPost, ActionName("Delete")]
        [JwtAuthorizationFilter]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }

        private int GetCompanyId()
        {
            return HttpContext.Session.GetInt32(SessionVariable.CompanyId).Value;
        }
    }
}
