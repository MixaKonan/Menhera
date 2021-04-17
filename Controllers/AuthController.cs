using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Menhera.Authentication;
using Menhera.Classes.Hash;
using Menhera.Classes.Logging;
using Menhera.Database;
using Menhera.Models.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using static Menhera.Classes.Logging.Logger;


namespace Menhera.Controllers
{
    public class AuthController : Controller
    {
        private readonly MenherachanContext _db;
        private readonly IWebHostEnvironment _env;

        private readonly string _logDirectory;


        public AuthController(MenherachanContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
            
            _logDirectory = Path.Combine(_env.WebRootPath, "logs", "auth_logs.log");

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
                var loginPassHash = await Authenticator.GetHashStringAsync(loginModel.Password);

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
                            await LogIntoFile(_logDirectory, string.Concat
                                    ("New login from: ", admin.ToString()),
                                LoggingInformationKind.Info);
                            return RedirectToAction("Main", "MainPage");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("SequenceContainsNoElements", "Вы не модератор.");
                    }
                }
                catch (Exception e)
                {
                    await LogIntoFile(_logDirectory, string.Concat(e.Message, "\n", e.StackTrace),
                        LoggingInformationKind.Error);
                    ModelState.AddModelError("SequenceContainsNoElements", "Мы не знаем такого модератора!");
                }
            }

            return View(loginModel);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Json(new { redirectToUrl = Url.Action("Login", "Auth") });
        }

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultNameClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}