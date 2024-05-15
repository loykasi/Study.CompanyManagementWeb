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
    }
}
