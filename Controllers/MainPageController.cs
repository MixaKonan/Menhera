using System.Collections.Generic;
using Menhera.Classes;
using Menhera.Classes.Db;
using Menhera.Database;
using Menhera.Intefaces;
using Menhera.Models;
using Microsoft.AspNetCore.Mvc;

namespace Menhera.Controllers
{
    public class MainPageController : Controller
    {
        private readonly IBoardCollection _collection;

        private readonly MenherachanContext _db;

        private readonly Dictionary<Board, BoardInformation> _boardInfo = new Dictionary<Board, BoardInformation>();
        // GET
        public IActionResult Main()
        {
            foreach (var board in _collection.Boards)
            {
                _boardInfo.Add(board, new BoardInformation(board, _db));
            }
            
            return View(_boardInfo);
        }

        public MainPageController(IBoardCollection collection, MenherachanContext db)
        {
            _collection = collection;
            _db = db;
        }
    }
}