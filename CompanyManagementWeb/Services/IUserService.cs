namespace CompanyManagementWeb.Services
{
    public interface IUserService
    {
        public bool IsLogged();
        public Task<bool> IsInPermission(string resource, string permission);
        public Task<bool> IsInCompany();
        public Task<bool> IsAdmin();
        public Task<bool> HasPermission();
    }
}