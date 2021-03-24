using Menhera.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Menhera.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        // GET
        public IActionResult Panel()
        {
            return View();
        }

        public IActionResult Create(BoardAdmin boardAdmin)
        {
            return View();
        }
        
        public IActionResult Admins()
        {
            return View();
        }
        
        public IActionResult Boards()
        {
            return View();
        }
    }
}