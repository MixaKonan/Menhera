using Microsoft.AspNetCore.Mvc;

namespace Menhera.Controllers
{
    public class BanController : Controller
    {
        public IActionResult YouAreBanned()
        {
            return View("Ban");
        }
    }
}