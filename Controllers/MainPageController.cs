using Menhera.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Menhera.Controllers
{
    public class MainPageController : Controller
    {
        // GET
        public IActionResult Main()
        {
            return View();
        }
    }
}