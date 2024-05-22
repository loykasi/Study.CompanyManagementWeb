using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CompanyManagementWeb.DataAccess;
using CompanyManagementWeb.Models;
using CompanyManagementWeb.ViewModels;
using CompanyManagementWeb.Data;
using CompanyManagementWeb.Attributes;

namespace CompanyManagementWeb.Controllers
{
    public class RoleController : Controller
    {
        private readonly CompanyManagementDbContext _context;
        private readonly ILogger<RoleController> _logger;

        public RoleController(CompanyManagementDbContext context, ILogger<RoleController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Role
        [JwtAuthorizationFilter(resource: ResourceEnum.Role, permission: PermissionEnum.View)]
        public async Task<IActionResult> Index()
        {
            RoleIndexViewModel roleIndexViewModel = new()
            {
                Roles = new List<RoleViewModel>()
            };

            var roles = await _context.Roles.Where(r => r.CompanyId == GetCompanyId()).ToListAsync();
            foreach (var role in roles)
            {
                RoleViewModel roleViewModel = new()
                {
                    Id = role.Id,
                    Name = role.Name,
                    IsAdmin = role.IsAdmin,
                    RoleDetails = await _context.RolePermissions.Include(r => r.Resource)
                                                            .Include(r => r.Permission)
                                                            .Where(r => r.RoleId == role.Id)
                                                            .Select(r => new RoleDetailViewModel
                                                            {
                                                                Resource = ResourceVariable.GetLocalizedName(r.Resource.Name),
                                                                Permission = PermissionVariable.GetLocalizedName(r.Permission.Name)
                                                            }).ToListAsync()
                };
                roleIndexViewModel.Roles.Add(roleViewModel);
            }

            return View(roleIndexViewModel);
        }

        // GET: Role/Create
        [JwtAuthorizationFilter(resource: ResourceEnum.Role, permission: PermissionEnum.Edit)]
        public async Task<IActionResult> Create()
        {
            RoleCreateViewModel roleCreateViewModel = new()
            {
                RoleDetails = await _context.Resources.Select(r => new RoleDetailCreateViewModel
                {
                    ResourceId = r.Id,
                    Resource = ResourceVariable.GetLocalizedName(r.Name),
                    Permissions = new SelectList(
                                        _context.Permissions.Select(p => new
                                            {
                                                Id = p.Id,
                                                Name = PermissionVariable.GetLocalizedName(p.Name)
                                            }).ToList(), 
                                        "Id", 
                                        "Name"
                                    )
                }).ToListAsync()
            };
            return View(roleCreateViewModel);
        }

        // POST: Role/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [JwtAuthorizationFilter(resource: ResourceEnum.Role, permission: PermissionEnum.Edit)]
        public async Task<IActionResult> Create(RoleCreateViewModel roleCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                Role role = new()
                {
                    Name = roleCreateViewModel.Name,
                    CompanyId = GetCompanyId(),
                    IsAdmin = roleCreateViewModel.IsAdmin
                };
                _context.Add(role);
                await _context.SaveChangesAsync();

                foreach (var item in roleCreateViewModel.RoleDetails)
                {
                    RolePermission rolePermission = new()
                    {
                        RoleId = role.Id,
                        ResourceId = item.ResourceId,
                        PermissionId = item.PermissionId
                    };
                    _context.Add(rolePermission);
                }
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            roleCreateViewModel.RoleDetails = await _context.Resources.Select(r => new RoleDetailCreateViewModel
                {
                    ResourceId = r.Id,
                    Resource = r.Name,
                    Permissions = new SelectList(
                                        _context.Permissions.Select(p => new
                                            {
                                                Id = p.Id,
                                                Name = PermissionVariable.GetLocalizedName(p.Name)
                                            }).ToList(), 
                                        "Id", 
                                        "Name"
                                    )
                }).ToListAsync();
            return View(roleCreateViewModel);
        }

        // GET: Role/Edit/5
        [JwtAuthorizationFilter(resource: ResourceEnum.Role, permission: PermissionEnum.Edit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            RoleCreateViewModel roleCreateViewModel = new()
            {
                Id = role.Id,
                Name = role.Name,
                IsAdmin = role.IsAdmin,
            };

            if (role.IsAdmin)
            {
                roleCreateViewModel.RoleDetails = await _context.Resources.Select(r => new RoleDetailCreateViewModel
                    {
                        ResourceId = r.Id,
                        Resource = ResourceVariable.GetLocalizedName(r.Name),
                        Permissions = new SelectList(
                                        _context.Permissions.Select(p => new
                                            {
                                                Id = p.Id,
                                                Name = PermissionVariable.GetLocalizedName(p.Name)
                                            }).ToList(), 
                                        "Id", 
                                        "Name"
                                    )
                    }).ToListAsync();
            }
            else
            {
                roleCreateViewModel.RoleDetails = [];
                var rolePermissions = await _context.RolePermissions.Where(r => r.RoleId == role.Id).ToListAsync();
                foreach (var item in rolePermissions)
                {
                    var resource = _context.Resources.FirstOrDefault(r => r.Id == item.ResourceId);
                    var permission = _context.Permissions.FirstOrDefault(p => p.Id == item.PermissionId);
                    roleCreateViewModel.RoleDetails.Add(new RoleDetailCreateViewModel
                    {
                        ResourceId = resource.Id,
                        Resource = ResourceVariable.GetLocalizedName(resource.Name),
                        PermissionId = permission.Id,
                        Permissions = new SelectList(
                                        _context.Permissions.Select(p => new
                                            {
                                                Id = p.Id,
                                                Name = PermissionVariable.GetLocalizedName(p.Name)
                                            }).ToList(), 
                                        "Id", 
                                        "Name"
                                    )
                    });
                }
            }
            
            return View(roleCreateViewModel);
        }

        // POST: Role/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [JwtAuthorizationFilter(resource: ResourceEnum.Role, permission: PermissionEnum.Edit)]
        public async Task<IActionResult> Edit(RoleCreateViewModel roleCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var role = await _context.Roles.FindAsync(roleCreateViewModel.Id);
                    role.Name = roleCreateViewModel.Name;
                    role.IsAdmin = roleCreateViewModel.IsAdmin;
                    await _context.SaveChangesAsync();

                    if (_context.RolePermissions.Any(r => r.RoleId == roleCreateViewModel.Id))
                    {
                        var rolePermissions = _context.RolePermissions.Where(r => r.RoleId == roleCreateViewModel.Id);
                        foreach (var item in roleCreateViewModel.RoleDetails)
                        {
                            var detail = rolePermissions.FirstOrDefault(r => r.ResourceId == item.ResourceId);
                            detail.PermissionId = item.PermissionId;
                        }
                    }
                    else
                    {
                        foreach (var item in roleCreateViewModel.RoleDetails)
                        {
                            RolePermission rolePermission = new()
                            {
                                RoleId = roleCreateViewModel.Id.Value,
                                ResourceId = item.ResourceId,
                                PermissionId = item.PermissionId
                            };
                            _context.Add(rolePermission);
                        }
                    }
                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(roleCreateViewModel.Id.Value))
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
            

            return View(roleCreateViewModel);
        }

        // GET: Role/Delete/5
        [JwtAuthorizationFilter(resource: ResourceEnum.Role, permission: PermissionEnum.Edit)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles
                .Include(r => r.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // POST: Role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [JwtAuthorizationFilter(resource: ResourceEnum.Role, permission: PermissionEnum.Edit)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }

        private int GetCompanyId()
        {
            return HttpContext.Session.GetInt32(SessionVariable.CompanyId).Value;
        }
    }
}
