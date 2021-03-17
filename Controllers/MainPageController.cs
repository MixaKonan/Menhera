using Menhera.Helpers;
using Menhera.Intefaces;
using Menhera.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Menhera.Controllers
{
    public class MainPageController : Controller
    {
        private IBoardCollection _collection;
        // GET
        public IActionResult Main()
        {
            return View();
        }

        public MainPageController(IBoardCollection collection)
        {
            _collection = collection;
        }
    }
}