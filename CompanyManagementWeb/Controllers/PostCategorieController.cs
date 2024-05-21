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
using CompanyManagementWeb.Attributes;
using CompanyManagementWeb.Data;

namespace CompanyManagementWeb.Controllers
{
    public class PostCategoryController : Controller
    {
        private readonly CompanyManagementDbContext _context;

        public PostCategoryController(CompanyManagementDbContext context)
        {
            _context = context;
        }

        // GET: PostCategorie
        [JwtAuthorizationFilter(resource: ResourceEnum.PostCategory, permission: PermissionEnum.View)]
        public async Task<IActionResult> Index()
        {
            int companyId = HttpContext.Session.GetInt32("companyId").Value;
            var PostCategories = _context.PostCategories.Where(d => d.CompanyId == companyId);
            return View(await PostCategories.ToListAsync());
        }

        // GET: PostCategorie/Details/5
        [JwtAuthorizationFilter(resource: ResourceEnum.PostCategory, permission: PermissionEnum.Edit)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postCategory = await _context.PostCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postCategory == null)
            {
                return NotFound();
            }

            return View(postCategory);
        }

        // GET: PostCategorie/Create
        [JwtAuthorizationFilter(resource: ResourceEnum.PostCategory, permission: PermissionEnum.Edit)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: PostCategorie/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [JwtAuthorizationFilter(resource: ResourceEnum.PostCategory, permission: PermissionEnum.Edit)]
        public async Task<IActionResult> Create(PostCategoryCreateViewModel postCategoryCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                PostCategory postCategory = new()
                {
                    Name = postCategoryCreateViewModel.Name,
                    CompanyId = HttpContext.Session.GetInt32("companyId").Value
                };
                _context.Add(postCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(postCategoryCreateViewModel);
        }

        // GET: PostCategorie/Edit/5
        [JwtAuthorizationFilter(resource: ResourceEnum.PostCategory, permission: PermissionEnum.Edit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postCategory = await _context.PostCategories.FindAsync(id);
            if (postCategory == null)
            {
                return NotFound();
            }
            PostCategoryCreateViewModel postCategoryCreateViewModel = new()
            {
                Id = postCategory.Id,
                Name = postCategory.Name,
            };
            return View(postCategoryCreateViewModel);
        }

        // POST: PostCategorie/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [JwtAuthorizationFilter(resource: ResourceEnum.PostCategory, permission: PermissionEnum.Edit)]
        public async Task<IActionResult> Edit(PostCategoryCreateViewModel postCategoryCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var postCategory = _context.PostCategories.FirstOrDefault(d => d.Id == postCategoryCreateViewModel.Id);
                    postCategory.Name = postCategoryCreateViewModel.Name;
                    _context.Update(postCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostCategoryExists(postCategoryCreateViewModel.Id))
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
            return View(postCategoryCreateViewModel);
        }

        // POST: PostCategorie/Delete/5
        [HttpPost]
        [JwtAuthorizationFilter(resource: ResourceEnum.PostCategory, permission: PermissionEnum.Edit)]
        public async Task<IActionResult> Delete(int id)
        {
            var postCategory = await _context.PostCategories.FindAsync(id);
            if (postCategory != null)
            {
                _context.PostCategories.Remove(postCategory);
            }

            await _context.SaveChangesAsync();

            int companyId = HttpContext.Session.GetInt32("companyId").Value;
            var PostCategories = _context.PostCategories.Where(d => d.CompanyId == companyId);
            return PartialView("PostCategoryListPartial", await PostCategories.ToListAsync());
        }

        private bool PostCategoryExists(int id)
        {
            return _context.PostCategories.Any(e => e.Id == id);
        }
    }
}
