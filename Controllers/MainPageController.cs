using Menhera.Intefaces;
using Microsoft.AspNetCore.Mvc;

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