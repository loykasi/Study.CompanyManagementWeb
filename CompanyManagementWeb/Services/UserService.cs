
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

        public bool IsInCompany()
        {
            return _dbContext.UserCompanies.Any(u => u.UserId == _httpContextAccessor.HttpContext.Session.GetInt32(SessionVariable.UserId));
        }

        public bool IsInPermission(string resource, string permission)
        {
            int userId = _httpContextAccessor.HttpContext.Session.GetInt32(SessionVariable.UserId).Value;
            int roleId = _dbContext.UserCompanies.FirstOrDefault(u => u.UserId == userId).RoleId ?? 0;
            bool isPermitted = _dbContext.RolePermissions
                                            .Include(r => r.Resource)
                                            .Include(r => r.Permission)
                                            .Any(r => r.RoleId == roleId 
                                                    && r.Resource.Name.Equals(resource) 
                                                    && r.Permission.Name.Equals(permission)
                                                );

            return isPermitted;
        }

        public bool IsLogged()
        {
            return _httpContextAccessor.HttpContext.Session.GetInt32(SessionVariable.UserId) != null;
        }
    }
}