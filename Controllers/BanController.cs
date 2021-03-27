using Microsoft.AspNetCore.Mvc;

namespace Menhera.Controllers
{
    public class BanController : Controller
    {
        // GET
        public IActionResult YouAreBanned()
        {
            return View("Ban");
        }
    }
}