
using CompanyManagementWeb.Data;
using CompanyManagementWeb.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagementWeb.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyManagementDbContext _dbContext;

        public UserService(IHttpContextAccessor httpContextAccessor, CompanyManagementDbContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        public async Task<bool> IsAdmin()
        {
            var userRole = await _dbContext.UserCompanies.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == _httpContextAccessor.HttpContext.Session.GetInt32(SessionVariable.UserId));
            return userRole.RoleId == null ? false : userRole.Role.IsAdmin;
        }

        public async Task<bool> IsInCompany()
        {
            return await _dbContext.UserCompanies.AnyAsync(u => u.UserId == _httpContextAccessor.HttpContext.Session.GetInt32(SessionVariable.UserId));
        }

        public async Task<bool> IsInPermission(string resource, string permission)
        {
            int userId = _httpContextAccessor.HttpContext.Session.GetInt32(SessionVariable.UserId).Value;
            var role = await _dbContext.UserCompanies.FirstOrDefaultAsync(u => u.UserId == userId);
            int roleId = role.RoleId ?? 0;
            var userPermission = _dbContext.RolePermissions
                                                .Include(r => r.Resource)
                                                .Include(r => r.Permission)
                                                .FirstOrDefault(r => r.RoleId == roleId && r.Resource.Name.Equals(resource));
            if (userPermission == null)
            {
                return false;
            }
            bool isPermitted = IsPermitted(userPermission.Permission.Name, permission);

            return isPermitted;
        }

        private bool IsPermitted(string userPermission, string permission)
        {
            return userPermission switch
            {
                null => false,
                "View" => permission.Equals("View"),
                "Edit" => permission.Equals("Edit") || permission.Equals("View"),
                _ => false,
            };
        }

        public bool IsLogged()
        {
            return _httpContextAccessor.HttpContext.Session.GetInt32(SessionVariable.UserId) != null;
        }

        public async Task<bool> HasPermission()
        {
            int userId = _httpContextAccessor.HttpContext.Session.GetInt32(SessionVariable.UserId).Value;
            var userCompany = await _dbContext.UserCompanies.FirstOrDefaultAsync(u => u.UserId == userId);

            if (userCompany.RoleId == null)
            {
                return false;
            }

            var role = await _dbContext.Roles.FindAsync(userCompany.RoleId);
            if (role == null)
            {
                return false;
            }
            if (role.IsAdmin)
            {
                return true;
            }

            if (userCompany.DepartmentId == null)
            {
                return false;
            }
            
            return true;
        }
    }
}