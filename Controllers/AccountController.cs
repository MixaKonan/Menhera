using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Menhera.Authentification;
using Menhera.Classes;
using Menhera.Classes.Hash;
using Menhera.Database;
using Menhera.Extensions;
using Menhera.Models;
using Menhera.Models.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Menhera.Controllers
{
    public class AccountController : Controller
    {
        private readonly MenherachanContext _db;
        private EmailService _email;

        public AccountController(MenherachanContext db)
        {
            _db = db;
            _email = new EmailService();
        }
        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login loginModel)
        {
            if (ModelState.IsValid)
            {
                var hashComp = new HashComparator();
                var loginPassHash = new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(loginModel.Password)).GetString();
                
                var user = _db.Users.First(u => u.Email == loginModel.Email);
                if (user != null)
                {
                    if (hashComp.CompareStringHashes(user.PasswordHash, loginPassHash))
                    {
                        ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                    }
                    
                    await Authenticate(loginModel.Email);
 
                    return RedirectToAction("Main", "MainPage");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(loginModel);
        }
        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
        
        [HttpGet]
        public IActionResult LeaveRequest()
        {
            return View("Request");
        }

        public IActionResult AfterRequest()
        {
            return View("AfterRequest");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LeaveRequest(Request requestModel)
        {
            var auth = new Authentificator();
            User user = null;
            
            if (ModelState.IsValid)
            {
                try
                {
                    user = _db.Users.First(usr => usr.Email == requestModel.Email);
                }
                catch (InvalidOperationException e)
                {
                    if (user == null)
                    {
                        await _db.Users.AddAsync(new User
                        {
                            Email = requestModel.Email,
                            Login = requestModel.Login,
                            PasswordHash = await auth.GetHashStringAsync(requestModel.Password)
                        });

                        await _db.SaveChangesAsync();

                        await _email.SendEmailAsync("fater181@gmail.com", "Заявка на модерство " + requestModel.Login, requestModel.Comment);
                        await _email.SendEmailAsync(requestModel.Email, "Заявка на модератора",
                            "Заявка принята и будет рассмотренна");

                        return View("AfterRequest");
                    }
                    ModelState.AddModelError("", "Проверьте введённые данные.");
                }
            }
            return RedirectToAction("LeaveRequest");
        }
        
        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}