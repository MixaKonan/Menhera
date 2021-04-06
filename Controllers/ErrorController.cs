using Microsoft.AspNetCore.Mvc;

namespace Menhera.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Error(int statusCode)
        {
            ViewBag.StatusCode = statusCode;
            
            return View();
        }
    }
}