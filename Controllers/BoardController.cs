using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Menhera.Intefaces;

namespace Menhera.Controllers
{
    public class BoardController : Controller
    {
        private IBoardCollection _collection;
        public IActionResult Board(string prefix)
        {
            var board = _collection.Boards.First(brd => brd.Prefix == prefix);
            
            return View(board);
        }

        public IActionResult Search(string prefix)
        {
            var board = _collection.Boards.First(brd => brd.Prefix == prefix);
            
            return View(board);
        }

        public BoardController(IBoardCollection collection)
        {
            _collection = collection;
        }
    }
}