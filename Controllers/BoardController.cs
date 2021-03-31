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
using Menhera.Classes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using File = Menhera.Models.File;

namespace Menhera.Controllers
{
    public class BoardController : Controller
    {
        private const double ThumbW = 200;
        private const double ThumbH = 200;
        private const int PageSize = 10;

        private readonly MenherachanContext _db;
        private readonly IBoardCollection _collection;
        private readonly IWebHostEnvironment _env;

        private readonly MD5CryptoServiceProvider _md5;

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

                    var thumbnailPath = Path.Combine(_env.WebRootPath, "thumbnails");

                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            using (var creator = new ImageThumbnailCreator(file, fileDirectory, thumbnailPath))
                            {
                                await creator.CreateThumbnail(ThumbW, ThumbH);

                                using (_db)
                                {
                                    _db.File.Add(new File
                                    {
                                        BoardId = post.BoardId,
                                        ThreadId = post.ThreadId,
                                        PostId = post.PostId,
                                        FileName = creator.FileName,
                                        ThumbnailName = creator.ThumbnailName,
                                        Info = $"{creator.ImageInfo}"
                                    });
                                    _db.SaveChanges();
                                }
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

                var threadPostPostsList = new List<ThreadPostLastThreePosts>();

                var boards = _db.Board.Where(b => b.Prefix == prefix).Include(b => b.Thread).ToList();

                Board board;
                if (boards.Count > 0)
                {
                    board = boards[0];
                    ViewBag.FileLimit = board.FileLimit;
                }
                else
                {
                    ModelState.AddModelError("SequenceContainsNoElements", "Такой доски не существует.");
                    Response.StatusCode = 404;
                    return RedirectToAction("Error", "Error", new {statusCode = 404});
                }

                foreach (var thread in board.Thread)
                {
                    var threads = _db.Thread.Where(t => t.ThreadId == thread.ThreadId).Include(t => t.Post).ToList();

                    Thread thrd;
                    if (threads.Count > 0)
                    {
                        thrd = threads[0];
                    }
                    else
                    {
                        ModelState.AddModelError("SequenceContainsNoElements", "Такой доски не существует.");
                        Response.StatusCode = 404;
                        return RedirectToAction("Error", "Error", new {statusCode = 404});  
                    }

                    var postList = thrd.Post.ToList();

                    if (postList.Count > 0)
                    {
                        var firstPost = postList[0];

                        var posts =
                            _db.Post.Where(p => p.PostId == firstPost.PostId).Include(p => p.File).ToList();

                        var firstPostFiles = new List<File>();

                        if (posts.Count > 0)
                        {
                            firstPostFiles = posts[0].File.ToList();
                        }

                        var threadPosts = _db.Thread.Where(t => t.ThreadId == thread.ThreadId)
                            .Include(t => t.Post).ToList()[0].Post;

                        var lTp = new List<Post>();
                        
                        if (threadPosts.Count > 3)
                        {
                            lTp = threadPosts.Take(3).ToArray().OrderByDescending(p => p.TimeInUnixSeconds).ToList();
                        }

                        var lTpFiles = new Dictionary<Post, List<File>>();

                        foreach (var post in lTp)
                        {
                            lTpFiles.Add(post, post.File.ToList());
                        }

                        threadPostPostsList.Add(new ThreadPostLastThreePosts
                        {
                            Thread = thread,

                            FirstPost = new KeyValuePair<Post, List<File>>(firstPost, firstPostFiles),

                            LastThreePosts = lTpFiles
                        });
                    }
                }

                var count = threadPostPostsList.Count;
                var items = threadPostPostsList.Skip((page - 1) * PageSize).Take(PageSize).ToList();

                var pageViewModel = new PageViewModel(count, page, PageSize);
                var threadViewModel = new ThreadViewModel
                {
                    PageViewModel = pageViewModel,
                    Model = items
                };

                ViewBag.ThreadViewModel = threadViewModel;
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

        public BoardController(MenherachanContext db, IWebHostEnvironment env, IBoardCollection collection)
        {
            _db = db;
            _env = env;
            _collection = collection;

            _md5 = new MD5CryptoServiceProvider();
        }
    }
}