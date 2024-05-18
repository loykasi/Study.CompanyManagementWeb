using CompanyManagementWeb.Data;
using CompanyManagementWeb.DataAccess;
using CompanyManagementWeb.Models;
using CompanyManagementWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManagementWeb.Controllers
{
    public class CompanyController : Controller
    {
        private readonly CompanyManagementDbContext _context;
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(CompanyManagementDbContext context, ILogger<CompanyController> logger)
        {
            _context = context;
            _logger = logger;
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

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Join()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Join(string code)
        {
            _logger.LogInformation("Company code: {code}", code);
            
            var company = _context.Companies.FirstOrDefault(c => c.Code == code);
            if (company == null)
            {
                return View();
            }

            UserCompany userCompany = new()
            {
                UserId = HttpContext.Session.GetInt32(SessionVariable.UserId).Value,
                CompanyId = company.Id
            };

            var companyContext = _context.UserCompanies.FirstOrDefault(u => u.UserId == userCompany.UserId);
            if (companyContext != null)
            {
                _context.Remove(companyContext);
            }
            _context.Add(userCompany);
            _context.SaveChanges();

            HttpContext.Session.SetInt32("companyId", company.Id);

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
