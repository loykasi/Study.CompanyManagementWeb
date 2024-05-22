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
            PostCategoryViewModel postCategoryViewModel = new()
            {
                PostCategories = await PostCategories.ToListAsync()
            };
            return View(postCategoryViewModel);
        }

        // POST: PostCategorie/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [JwtAuthorizationFilter(resource: ResourceEnum.PostCategory, permission: PermissionEnum.Edit)]
        public async Task<IActionResult> Create(PostCategoryViewModel postCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                PostCategory postCategory = new()
                {
                    Name = postCategoryViewModel.PostCategoryCreateViewModel.Name,
                    CompanyId = HttpContext.Session.GetInt32("companyId").Value
                };
                _context.Add(postCategory);
                await _context.SaveChangesAsync();

                int companyId = HttpContext.Session.GetInt32("companyId").Value;
                var PostCategories = _context.PostCategories.Where(d => d.CompanyId == companyId);
                postCategoryViewModel.PostCategories = await PostCategories.ToListAsync();
                return PartialView("PostCategoryListPartial", postCategoryViewModel);
            }
            return NoContent();
        }

        // POST: PostCategorie/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [JwtAuthorizationFilter(resource: ResourceEnum.PostCategory, permission: PermissionEnum.Edit)]
        public async Task<IActionResult> Edit(PostCategoryViewModel postCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var postCategory = _context.PostCategories.FirstOrDefault(d => d.Id == postCategoryViewModel.PostCategoryCreateViewModel.Id);
                    postCategory.Name = postCategoryViewModel.PostCategoryCreateViewModel.Name;
                    _context.Update(postCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostCategoryExists(postCategoryViewModel.PostCategoryCreateViewModel.Id))
                    {
                        return NoContent();
                    }
                    else
                    {
                        throw;
                    }
                }
                
                int companyId = HttpContext.Session.GetInt32("companyId").Value;
                var PostCategories = _context.PostCategories.Where(d => d.CompanyId == companyId);
                postCategoryViewModel.PostCategories = await PostCategories.ToListAsync();
                return PartialView("PostCategoryListPartial", postCategoryViewModel);
            }
            return NoContent();
        }

        // POST: PostCategorie/Delete/5
        [HttpPost]
        [JwtAuthorizationFilter(resource: ResourceEnum.PostCategory, permission: PermissionEnum.Edit)]
        public async Task<IActionResult> Delete(int id)
        {
            var postCategory = await _context.PostCategories.FindAsync(id);
            if (postCategory != null)
            {
                var posts = _context.Posts.Where(d => d.PostCategoryId == postCategory.Id);
                foreach (var item in posts)
                {
                    item.PostCategoryId = null;
                }

                _context.PostCategories.Remove(postCategory);
            }

            await _context.SaveChangesAsync();

            int companyId = HttpContext.Session.GetInt32("companyId").Value;
            var PostCategories = _context.PostCategories.Where(d => d.CompanyId == companyId);
            PostCategoryViewModel postCategoryViewModel = new()
            {
                PostCategories = await PostCategories.ToListAsync()
            };
            return PartialView("PostCategoryListPartial", postCategoryViewModel);
        }

        private bool PostCategoryExists(int id)
        {
            return _context.PostCategories.Any(e => e.Id == id);
        }
    }
}
