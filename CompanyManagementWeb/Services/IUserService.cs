namespace CompanyManagementWeb.Services
{
    public interface IUserService
    {
        public bool IsLogged();
        public bool IsInPermission(string resource, string permission);
        public bool IsInCompany();
        public Task<bool> IsAdmin();
    }
}