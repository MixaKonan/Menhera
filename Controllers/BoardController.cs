using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Menhera.Database;
using Menhera.Extensions;
using Menhera.Intefaces;
using Menhera.Models;
using Menhera.Classes.Anon;
using Menhera.Classes.Db;
using Menhera.Classes.Files;
using Menhera.Classes.Pagination;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using File = Menhera.Models.File;

namespace Menhera.Controllers
{
    public class BoardController : Controller
    {
        private const int PageSize = 10;

        private readonly MenherachanContext _db;
        private readonly IBoardCollection _collection;
        private readonly IWebHostEnvironment _env;

        private readonly MD5CryptoServiceProvider _md5;

        public BoardController(MenherachanContext db, IWebHostEnvironment env, IBoardCollection collection)
        {
            _db = db;
            _env = env;
            _collection = collection;

            _md5 = new MD5CryptoServiceProvider();
        }

        [HttpPost]
        public async Task<IActionResult> AddThread(Post post, List<IFormFile> files)
        {
            var ipHash = _md5.ComputeHash(HttpContext.Connection.RemoteIpAddress.GetAddressBytes())
                .GetString();

            var anon = new Anon(ipHash, IpCheck.UserIsBanned(_db, ipHash));

            if (anon.IsBanned)
            {
                return RedirectToAction("YouAreBanned", "Ban");
            }

            if (ModelState.IsValid)
            {
                post.AnonIpHash = ipHash;
                DbAccess.AddThreadToBoard(_db, ref post);

                if (files.Count > 0)
                {
                    var fileDirectory = Path.Combine(_env.WebRootPath, "images");

                    var thumbNailDirectory = Path.Combine(_env.WebRootPath, "thumbnails");

                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            using (var creator = new ImageThumbnailCreator(file, fileDirectory, thumbNailDirectory))
                            {
                                await creator.CreateThumbnailAsync();

                                DbAccess.AddFilesToPost(_db, post, creator.ImgInfo);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("FileLengthNotValid", "Файл пустой.");
                        }
                    }
                }

                return RedirectToAction("Thread", "Thread", new {id = post.ThreadId});
            }

            return RedirectToAction("Board");
        }

        [HttpGet]
        public IActionResult Board(string prefix, int page = 1)
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

            ViewBag.Id = 0;

            ViewBag.Page = page;

            try
            {
                ViewBag.Board = _collection.Boards.First(brd => brd.Prefix == prefix);

                var allThreads = new List<KeyValuePair<Thread, List<KeyValuePair<Post, List<File>>>>>();

                var boards = _db.Board.Where(b => b.Prefix == prefix).Include(b => b.Thread).ToList();

                Board board;
                if (boards.Count > 0)
                {
                    board = boards[0];
                    ViewBag.FileLimit = board.FileLimit;
                }
                else
                {
                    Response.StatusCode = 404;
                    return RedirectToAction("Error", "Error", new {statusCode = 404});
                }

                foreach (var thrd in board.Thread)
                {
                    var thread = _db.Thread.Include(t => t.Post).First(t => t.ThreadId == thrd.ThreadId);

                    if (thread != null)
                    {
                        var postFiles = new List<KeyValuePair<Post, List<File>>>();

                        if (thread.Post.Count >= 4)
                        {
                            var posts = new List<Post> {thread.Post.ToArray()[0]};

                            posts.AddRange(thread.Post.ToList().OrderByDescending(p => p.TimeInUnixSeconds).Take(3));

                            foreach (var post in posts)
                            {
                                var p = _db.Post.Include(pp => pp.File).First(pp => pp.PostId == post.PostId);
                                postFiles.Add(new KeyValuePair<Post, List<File>>(p, p.File.ToList()));
                            }

                            allThreads.Add(
                                new KeyValuePair<Thread, List<KeyValuePair<Post, List<File>>>>(thread, postFiles));
                        }
                        else if (thread.Post.Count > 0 && thread.Post.Count <= 3)
                        {
                            var p = _db.Post.Include(pp => pp.File).First(pp => pp.ThreadId == thread.ThreadId);
                            postFiles.Add(new KeyValuePair<Post, List<File>>(p, p.File.ToList()));

                            allThreads.Add(
                                new KeyValuePair<Thread, List<KeyValuePair<Post, List<File>>>>(thread, postFiles));
                        }
                    }
                }

                var pageInfo = new PageInfo( page, PageSize, allThreads.Count);

                var pageThreads = new List<KeyValuePair<Thread, List<KeyValuePair<Post, List<File>>>>>();


                for (var i = (page - 1) * PageSize; i < page * PageSize; i++)
                {
                    try
                    {
                        pageThreads.Add(allThreads[i]);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        break;
                    }
                }

                ViewBag.PageInfo = pageInfo;
                
                ViewBag.BoardViewModel = pageThreads;
            }
            catch (InvalidOperationException)
            {
                Response.StatusCode = 404;
                return RedirectToAction("Error", "Error", new {statusCode = 404});
            }

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
    }
}