using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Cryptography;
using Menhera.Database;
using Menhera.Extensions;
using Menhera.Intefaces;
using Menhera.Models;

namespace Menhera.Controllers
{
    public class BoardController : Controller
    {
        private readonly MenherachanContext _db;
        private readonly IBoardCollection _collection;


        // public IActionResult Board(Post post)
        // {
        //     //var id = _db.Thread.Where(t => t.)
        //     
        //     return RedirectToAction("Thread", thread.ThreadId);
        // }
        //
        public IActionResult Board(string prefix)
        {
            
            ViewBag.Board = _collection.Boards.First(brd => brd.Prefix == prefix);
            ViewBag.UserIp =
                new MD5CryptoServiceProvider().ComputeHash(HttpContext.Connection.RemoteIpAddress.GetAddressBytes()).GetString();

            ViewBag.ThreadPosts = new Dictionary<Thread, List<Post>>();

            var threads = _db.Thread.Select(thread => thread).ToList();

            foreach (var thread in threads)
            {
                ViewBag.ThreadPosts.Add(thread, _db.Thread.Join(_db.Post,
                    t => t.ThreadId,
                    p => p.ThreadId,
                    (th, pt) => new Post
                    {
                        BoardId = pt.BoardId,
                        ThreadId = pt.ThreadId,
                        PostId = pt.PostId,
                        AnonIpHash = pt.AnonIpHash,
                        AnonName = pt.AnonName,
                        Comment = pt.Comment,
                        BumpInUnixTime = pt.BumpInUnixTime,
                        Email = pt.Email,
                        IsPinned = pt.IsPinned
                    }).ToList());
            }


            return View();
        }


        public IActionResult Thread(int id)
        {
            return View();
        }

        [HttpPost]
        public void Report(int postId)
        {
            
        }
        
        public IActionResult Search(string prefix)
        {
            var board = _collection.Boards.First(brd => brd.Prefix == prefix);
            
            return View(board);
        }

        public BoardController(MenherachanContext db, IBoardCollection collection)
        {
            _db = db;
            _collection = collection;
        }
    }
}