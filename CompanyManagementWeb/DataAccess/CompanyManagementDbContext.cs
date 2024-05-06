using Microsoft.EntityFrameworkCore;

namespace CompanyManagementWeb.DataAccess
{
    public class CompanyManagementDbContext : DbContext
    {
        public CompanyManagementDbContext(DbContextOptions<CompanyManagementDbContext> options) : base(options)
        { }
    }
}
