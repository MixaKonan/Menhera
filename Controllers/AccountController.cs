using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Menhera.Database;
using Menhera.Extensions;
using Menhera.Models;
using Menhera.Models.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Menhera.Controllers
{
    public class AccountController : Controller
    {
        private readonly MenherachanContext _db;

        public AccountController(MenherachanContext db)
        {
            _db = db;
        }
        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> Login(Login loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == loginModel.Email && u.PasswordHash == GetHashStringAsync(loginModel.Password));
                if (user != null)
                {
                    await Authenticate(model.Email); // аутентификация
 
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Registration registrationModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _db.Users.FirstAsync(usr => usr.Email == registrationModel.Email);
                
                if (user == null)
                {
                    await _db.Users.AddAsync(new User
                    {
                        Email = registrationModel.Email,
                        Login = registrationModel.Login,
                        PasswordHash = await GetHashStringAsync(registrationModel.Password)
                    });

                    await _db.SaveChangesAsync();

                    await Authenticate(registrationModel.Email);
                    
                    return RedirectToAction("Main", "MainPage");
                }
                else
                {
                    ModelState.AddModelError("", "Проверьте введённые данные.");
                }
            }

            return RedirectToAction("Register");


        }
        
        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            
            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        
        
    }
}