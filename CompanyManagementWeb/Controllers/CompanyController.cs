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
        public async Task<IActionResult> Create(CompanyCreateViewModel companyCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(companyCreateViewModel);
            }
            int userId = HttpContext.Session.GetInt32("userId").Value;

            Company company = new()
            {
                Name = companyCreateViewModel.Name,
                Address = companyCreateViewModel.Address,
                CreatedDate = DateTime.Now,
            };
            _context.Add(company);
            await _context.SaveChangesAsync();

            Role role = new()
            {
                Name = "Admin",
                CompanyId = company.Id,
                IsAdmin = true
            };
            _context.Add(role);
            await _context.SaveChangesAsync();

            UserCompany userCompany = new()
            {
                UserId = userId,
                CompanyId = company.Id,
                RoleId = role.Id
            };
            company.Code = GenerateCompanyCode(company.Id);
            _context.Add(userCompany);
            await _context.SaveChangesAsync();

            HttpContext.Session.SetInt32("companyId", company.Id);
            HttpContext.Session.SetString("companyName", company.Name);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Join()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Join(CompanyJoinViewModel companyJoinViewModel)
        {
            _logger.LogInformation("Company code: {code}", companyJoinViewModel.Code);
            
            var company = _context.Companies.FirstOrDefault(c => c.Code == companyJoinViewModel.Code);
            if (company == null)
            {
                ModelState.AddModelError("CodeError", "Sai mã mời");
                return View();
            }

            UserCompany userCompany = new()
            {
                UserId = HttpContext.Session.GetInt32(SessionVariable.UserId).Value,
                CompanyId = company.Id
            };
            _context.Add(userCompany);

            var companyContext = _context.UserCompanies.FirstOrDefault(u => u.UserId == userCompany.UserId);
            if (companyContext != null)
            {
                _context.Remove(companyContext);
            }
            
            await _context.SaveChangesAsync();

            HttpContext.Session.SetInt32("companyId", company.Id);
            HttpContext.Session.SetString("companyName", company.Name);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Information()
        {
            var companyId = HttpContext.Session.GetInt32("companyId");
            if (companyId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var company = await _context.Companies.FindAsync(companyId);

            CompanyViewModel companyViewModel = new()
            {
                Name = company.Name,
                Address = company.Address,
                InviteCode = company.Code
            };
            return View(companyViewModel);
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
