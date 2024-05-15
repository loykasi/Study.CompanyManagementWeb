using System.IdentityModel.Tokens.Jwt;
using System.Text;
using CompanyManagementWeb.DataAccess;
using CompanyManagementWeb.Models;
using CompanyManagementWeb.ViewModels;
using CompanyManagementWeb.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CompanyManagementWeb.Controllers
{
    public class IdentityController : Controller
    {
        private readonly CompanyManagementDbContext _context;
        private readonly IConfiguration _configuration;

        public IdentityController(CompanyManagementDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!registerViewModel.Password.Equals(registerViewModel.ConfirmedPassword))
                {
                    return View(registerViewModel);
                }

                var passwordHasher = new PasswordHasher<User>();

                User user = new()
                {
                    Name = registerViewModel.Name,
                    Email = registerViewModel.Email
                };
                user.PasswordHash = passwordHasher.HashPassword(user, registerViewModel.Password);

                _context.Add(user);
                _context.SaveChanges();

                var accessToken = JwtHelper.GenerateJSONWebToken(_configuration);
                SetJWTCookie(accessToken);

                return RedirectToAction(nameof(Index), "Home");
            }

            return View(registerViewModel);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("model invalid", "(LOG)");
                return View(loginViewModel);
            }

            var user = _context.Users.FirstOrDefault(x => x.Email == loginViewModel.Email);
            if (user == null)
            {
                System.Diagnostics.Debug.WriteLine("no user", "(LOG)");
                return View(loginViewModel);
            }

            var passwordHasher = new PasswordHasher<User>();
            PasswordVerificationResult result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginViewModel.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                System.Diagnostics.Debug.WriteLine("password not match", "(LOG)");
                return View(loginViewModel);
            }

            var accessToken = JwtHelper.GenerateJSONWebToken(_configuration);
            SetJWTCookie(accessToken);

            return RedirectToAction(nameof(Index), "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));
        }
 
        private void SetJWTCookie(string token)
        {
            // var cookieOptions = new CookieOptions
            // {
            //     HttpOnly = true,
            //     Expires = DateTime.UtcNow.AddHours(3),
            // };
            // Response.Cookies.Append("jwtCookie", token, cookieOptions);
            System.Diagnostics.Debug.WriteLine("JWT: " + token, "(LOG)");
            HttpContext.Session.SetString("Token", token);
        }
    }
}