using CompanyManagementWeb.DataAccess;
using CompanyManagementWeb.Models;
using CompanyManagementWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManagementWeb.Controllers
{
    public class CompanyController : Controller
    {
        private readonly CompanyManagementDbContext _context;

        public CompanyController(CompanyManagementDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CompanyCreateViewModel companyCreateViewModel)
        {
            int userId = HttpContext.Session.GetInt32("userId").Value;

            Company company = new()
            {
                Name = companyCreateViewModel.Name,
            };
            _context.Add(company);
            _context.SaveChanges();

            UserCompany userCompany = new()
            {
                UserId = userId,
                CompanyId = company.Id
            };
            company.Code = GenerateCompanyCode(company.Id);
            _context.Add(userCompany);
            _context.SaveChanges();

            HttpContext.Session.SetInt32("companyId", company.Id);
            System.Diagnostics.Debug.WriteLine("Set company Id: " + company.Id, "SESSION");

            return RedirectToAction("Index", "Home");
        }

        private string GenerateCompanyCode(int id)
        {
            Guid guid = Guid.NewGuid();
            string code = Convert.ToBase64String(guid.ToByteArray());
            code = code.Replace("=","");
            code = code.Replace("+","");
            code = id + code;
            return code;
        }
    }
}
