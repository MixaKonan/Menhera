using System.Linq;
using Menhera.Database;
using Menhera.Models;
using Microsoft.AspNetCore.Mvc;

namespace Menhera.Controllers
{
    public class ThreadController : Controller
    {
        private readonly MenherachanContext _db;

        public ThreadController(MenherachanContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Thread(int id)
        {
            var thread = _db.Thread.First(t => t.ThreadId == id);
            
            ViewBag.Posts = _db.Thread.Where(t => t.ThreadId == thread.ThreadId).Join(_db.Post,
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
                }).ToList();
            
            return View(thread);
        }
    }
}