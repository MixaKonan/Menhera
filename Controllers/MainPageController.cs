using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Menhera.Classes.Db;
using Menhera.Classes.Logging;
using Menhera.Database;
using Menhera.Intefaces;
using Menhera.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using static Menhera.Classes.Logging.Logger;

namespace Menhera.Controllers
{
    public class MainPageController : Controller
    {
        private readonly IBoardCollection _collection;
        private readonly MenherachanContext _db;
        private readonly IWebHostEnvironment _env;
        
        private readonly string _logDirectory;

        private readonly Dictionary<Board, BoardInformation> _boardInfo = new Dictionary<Board, BoardInformation>();
        
        public MainPageController(MenherachanContext db, IBoardCollection collection, IWebHostEnvironment env)
        {
            _db = db;
            _collection = collection;
            _env = env;
            
            _logDirectory = Path.Combine(_env.WebRootPath, "logs", "main_page_logs.log");
        }

        public async Task<IActionResult> Main()
        {
            try
            {
                foreach (var board in _collection.Boards)
                {
                    _boardInfo.Add(board, new BoardInformation(board, _db));
                }

                return View(_boardInfo);
            }
            catch (Exception e)
            {
                await LogIntoFile(_logDirectory, string.Concat(e.Message, "\n", e.StackTrace),
                    LoggingInformationKind.Error);
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }
    }
}