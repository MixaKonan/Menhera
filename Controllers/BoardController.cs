using Microsoft.AspNetCore.Mvc;

namespace Menhera.Controllers
{
    public class BoardController : Controller
    {
        public IActionResult Board(string prefix)
        {
            ViewBag.Prefix = prefix;
            return View();
        }
    }
}