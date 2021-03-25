using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Cryptography;
using Menhera.Database;
using Menhera.Extensions;
using Menhera.Intefaces;
using Menhera.Models;
using Newtonsoft.Json.Converters;

namespace Menhera.Controllers
{
    public class BoardController : Controller
    {
        private readonly MenherachanContext _db;
        private readonly IBoardCollection _collection;


        [HttpPost]
        public IActionResult Board(Post post)
        {
            var thread = new Thread
            {
                AnonName = post.AnonName,
                BoardId = post.BoardId,
                OpIpHash = post.AnonIpHash,
                IsClosed = false,
            };

            _db.Thread.Add(thread);
            _db.SaveChangesAsync();

            _db.Post.Add(new Post
            {
                ThreadId = thread.ThreadId,
                AnonName = post.AnonName,
                AnonIpHash = post.AnonIpHash,
                BoardId = post.BoardId,
                Time = DateTime.Now,
                BumpInUnixTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
                Comment = post.Comment,
                Email = post.Email
            });

            _db.SaveChanges();

            return RedirectToAction("Thread", thread.ThreadId);
        }

        [HttpGet]
        public IActionResult Board(string prefix)
        {
            ViewBag.Board = _collection.Boards.First(brd => brd.Prefix == prefix);
            ViewBag.UserIp =
                new MD5CryptoServiceProvider().ComputeHash(HttpContext.Connection.RemoteIpAddress.GetAddressBytes())
                    .GetString();

            ViewBag.ThreadPosts = new Dictionary<Thread, List<Post>>();

            var threads = _db.Thread.Join(_db.Board.Where(b => b.Prefix == prefix),
                t => t.BoardId, b => b.BoardId, (thread, board) => new Thread
                {
                    ThreadId = thread.ThreadId,
                    BoardId = thread.BoardId,
                    IsClosed = thread.IsClosed,
                    OpIpHash = thread.OpIpHash,
                    AnonName = thread.AnonName
                }).ToList();

            foreach (var thread in threads)
            {
                ViewBag.ThreadPosts.Add(thread, _db.Thread.Where(t => t.ThreadId == thread.ThreadId).Join(_db.Post,
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