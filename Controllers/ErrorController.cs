using Microsoft.AspNetCore.Mvc;

namespace Menhera.Controllers
{
    public class ErrorController : Controller
    {
        // GET
        public IActionResult Error()
        {
            return View();
        }
    }
}