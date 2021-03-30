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
using System.Drawing;
using System.Drawing.Imaging;
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
                var thread = new Thread
                {
                    AnonName = post.AnonName,
                    BoardId = post.BoardId,
                    OpIpHash = ipHash,
                    IsClosed = false,
                };

                _db.Thread.Add(thread);
                _db.SaveChanges();

                var addPost = new Post
                {
                    ThreadId = thread.ThreadId,
                    AnonName = post.AnonName,
                    AnonIpHash = ipHash,
                    BoardId = post.BoardId,
                    Subject = post.Subject,
                    TimeInUnixSeconds = DateTimeOffset.Now.ToUnixTimeSeconds(),
                    Comment = post.Comment,
                    Email = post.Email
                };

                _db.Post.Add(addPost);
                _db.SaveChanges();

                if (files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            var extension = Path.GetExtension(file.FileName);

                            var filePath = Path.Combine(_env.WebRootPath, "images");
                            var fullFilePath = string.Concat(filePath, "\\", file.FileName);

                            await using (var stream = new FileStream(fullFilePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            var thumbnailName = string.Concat(new Random().Next(int.MaxValue).ToString(), extension);
                            var thumbnailPath = Path.Combine(_env.WebRootPath, "thumbnails");
                            var fullThumbnailPath = string.Concat(thumbnailPath, "\\", thumbnailName);

                            if (extension == ".jpeg" || extension == ".jpg" || extension == ".png")
                            {
                                var image = Image.FromFile(fullFilePath);

                                if (image.Width < 200 && image.Height < 200)
                                {
                                    await using (var stream = new FileStream(fullThumbnailPath, FileMode.Create))
                                    {
                                        await file.CopyToAsync(stream);
                                    }
                                }
                                else
                                {
                                    var coefficient = image.Width > image.Height
                                        ? ThumbW / image.Width
                                        : ThumbH / image.Height;
                                    var thumbnailHeight = (int) Math.Round(image.Height * coefficient,
                                        MidpointRounding.ToEven);
                                    var thumbnailWidth = (int) Math.Round(image.Width * coefficient,
                                        MidpointRounding.ToEven);

                                    var thumbnail = image.GetThumbnailImage(thumbnailWidth, thumbnailHeight,
                                        () => false,
                                        IntPtr.Zero);

                                    await using (var stream = new FileStream(fullThumbnailPath, FileMode.Create))
                                    {
                                        thumbnail.Save(stream, ImageFormat.Jpeg);
                                    }
                                }

                                _db.File.Add(new File
                                {
                                    BoardId = addPost.BoardId,
                                    ThreadId = addPost.ThreadId,
                                    PostId = addPost.PostId,
                                    FileName = file.FileName,
                                    ThumbnailName = thumbnailName,
                                    Info = $"{image.Size}"
                                });
                                _db.SaveChanges();
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("FileLengthNotValid", "Файл пустой.");
                        }
                    }
                }

                return RedirectToAction("Thread", "Thread", new {id = thread.ThreadId});
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

                        var lTp = _db.Thread.Where(t => t.ThreadId == thread.ThreadId)
                            .Include(t => t.Post).ToList()[0].Post
                            .Take(3).ToArray().OrderByDescending(p => p.TimeInUnixSeconds).ToArray();

                        var lTpFiles = new Dictionary<Post, List<File>>();

                        foreach (var post in lTp)
                        {
                            lTpFiles.Add(post, _db.Post.Where(p => p.PostId == post.PostId).Join(_db.File,
                                p => p.PostId,
                                f => f.PostId,
                                (pt, file) => new File
                                {
                                    FileId = file.FileId,
                                    BoardId = file.BoardId,
                                    ThreadId = file.ThreadId,
                                    PostId = file.PostId,
                                    FileName = file.FileName,
                                    ThumbnailName = file.ThumbnailName,
                                    Info = file.Info
                                }).ToList());
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