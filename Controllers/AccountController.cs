using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Menhera.Classes.Hash;
using Menhera.Database;
using Menhera.Extensions;
using Menhera.Models.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Login(Login loginModel)
        {
            if (ModelState.IsValid)
            {
                var loginPassHash = new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(loginModel.Password))
                    .GetString();

                try
                {
                    var admin = _db.Admin.First(a => a.Email == loginModel.Email);
                    
                    if (admin != null)
                    {
                        if (!HashComparator.CompareStringHashes(admin.PasswordHash, loginPassHash))
                        {
                            ModelState.AddModelError("WrongPassword", "Некорректный пароль.");
                        }
                        else
                        {
                            await Authenticate(loginModel.Email);

                            return RedirectToAction("Main", "MainPage");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("SequenceContainsNoElements", "Вы не модератор.");
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("SequenceContainsNoElements", "Мы не знаем такого модератора!");
                }
            }

            return View(loginModel);
        }
        
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultNameClaimType);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}