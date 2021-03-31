using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Menhera.Classes;
using Menhera.Database;
using Menhera.Extensions;
using Menhera.Intefaces;
using Menhera.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Menhera.Controllers
{
    public class ThreadController : Controller
    {
        private readonly MenherachanContext _db;
        private readonly IBoardCollection _collection;
        private readonly IWebHostEnvironment _env;
        
        private readonly MD5CryptoServiceProvider _md5;


        public ThreadController(MenherachanContext db, IWebHostEnvironment env, IBoardCollection collection)
        {
            _db = db;
            _env = env;
            _collection = collection;

            _md5 = new MD5CryptoServiceProvider();
        }

        [HttpGet]
        public IActionResult Thread(int id)
        {
            var ipHash = _md5.ComputeHash(HttpContext.Connection.RemoteIpAddress.GetAddressBytes())
                .GetString();

            var anon = new Anon(ipHash, IpCheck.UserIsBanned(_db, ipHash));

            ViewBag.UserIpHash = anon.IpHash;

            ViewBag.UserIsBanned = false;
            
            ViewBag.Id = 0;

            if (anon.IsBanned)
            {
                ViewBag.UserIsBanned = anon.IsBanned;
                var ban = _db.Ban.First(b => b.AnonIpHash == anon.IpHash);
                ViewBag.BanReason = ban.Reason;
                ViewBag.BanEnd = DateTimeOffset.FromUnixTimeSeconds(ban.Term).ToLocalTime();
            }

            try
            {
                using (_db)
                {
                    try
                    {
                        var thread = _db.Thread.Where(t => t.ThreadId == id).
                            Include(t => t.Board).
                            Include(t => t.Post).ToList()[0];

                        var posts = _db.Post.Where(p => p.ThreadId == id).Include(p => p.File).ToList();

                        var postsFiles = new Dictionary<Post, List<File>>();

                        foreach (var post in posts)
                        {
                            postsFiles.Add(post, post.File.ToList());
                        }

                        ViewBag.Board = thread.Board;
                        ViewBag.Thread = thread;
                        ViewBag.PostsFiles = postsFiles;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Response.StatusCode = 404;
                        return RedirectToAction("Error", "Error", new {statusCode = Response.StatusCode});
                    }
                   
                }
            }
            catch (InvalidOperationException)
            {
                Response.StatusCode = 500;
                return RedirectToAction("Error", "Error", new {statusCode = Response.StatusCode});
            }
            return View();
        }

        [HttpPost]
        public IActionResult AddPost(Post post)
        {
            try
            {
                var ipHash = _md5.ComputeHash(HttpContext.Connection.RemoteIpAddress.GetAddressBytes())
                    .GetString();

                var anon = new Anon(ipHash, IpCheck.UserIsBanned(_db, ipHash));

                ViewBag.UserIpHash = anon.IpHash;

                ViewBag.UserIsBanned = false;

                if (anon.IsBanned)
                {
                    ViewBag.UserIsBanned = anon.IsBanned;
                    var ban = _db.Ban.First(b => b.AnonIpHash == anon.IpHash);
                    ViewBag.BanReason = ban.Reason;
                    ViewBag.BanEnd = DateTimeOffset.FromUnixTimeSeconds(ban.Term).ToLocalTime();
                }

                post.AnonIpHash = ipHash;
                post.TimeInUnixSeconds = DateTimeOffset.Now.ToUnixTimeSeconds();
                
                DbAccess.AddPostToThread(_db, post);
            }
            catch (InvalidOperationException)
            {
                Response.StatusCode = 404;
                return RedirectToAction("Error", "Error");
            }

            return RedirectToAction("Thread", new {id = post.ThreadId});
        }
    }
}