using CompanyManagementWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagementWeb.DataAccess
{
    public class CompanyManagementDbContext : DbContext
    {
        public CompanyManagementDbContext(DbContextOptions<CompanyManagementDbContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<PostCategory> PostCategories { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<UserCompany> UserCompanies { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
    }
}
