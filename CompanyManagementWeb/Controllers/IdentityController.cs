using CompanyManagementWeb.DataAccess;
using CompanyManagementWeb.Models;
using CompanyManagementWeb.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CompanyManagementWeb.Services;

namespace CompanyManagementWeb.Controllers
{
    public class IdentityController : Controller
    {
        private readonly CompanyManagementDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public IdentityController(CompanyManagementDbContext context, IConfiguration configuration, ITokenService tokenService)
        {
            _context = context;
            _configuration = configuration;
            _tokenService = tokenService;
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

                var token = _tokenService.GenerateToken(user);
                SetJWTCookie(token.AccessToken!);
                SetRefreshTokenCookie(user, token.RefreshToken!);
                HttpContext.Session.SetInt32("userId", user.Id);

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

            var token = _tokenService.GenerateToken(user);
            SetJWTCookie(token.AccessToken!);
            SetRefreshTokenCookie(user, token.RefreshToken!);
            HttpContext.Session.SetInt32("userId", user.Id);

            return RedirectToAction(nameof(Index), "Home");
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwtCookie");
            Response.Cookies.Delete("refreshTokenCookie");
            HttpContext.Session.Clear();
            var user = _context.Users.FirstOrDefault(u => u.Id == HttpContext.Session.GetInt32("userId"));
            if (user != null)
            {
                user.RefreshToken = null;
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
 
        private void SetJWTCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(3),
            };
            Response.Cookies.Append("jwtCookie", token, cookieOptions);
        }

        private void SetRefreshTokenCookie(User user, string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7),
            };
            Response.Cookies.Append("refreshTokenCookie", token, cookieOptions);

            user.RefreshToken = token;
            _context.SaveChanges();
        }
    }
}