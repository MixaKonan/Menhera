using Microsoft.AspNetCore.Mvc;

namespace Menhera.Controllers
{
    public class AdminController : Controller
    {
        // GET
        public IActionResult Panel()
        {
            return View();
        }
    }
}