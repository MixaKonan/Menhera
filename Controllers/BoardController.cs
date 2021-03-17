using System;
using Menhera.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using Menhera.Helpers;
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

        public BoardController(IBoardCollection collection)
        {
            _collection = collection;
        }
    }
}