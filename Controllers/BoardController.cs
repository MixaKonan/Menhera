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
using Microsoft.Extensions.Hosting;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using File = Menhera.Models.File;

namespace Menhera.Controllers
{
    public class BoardController : Controller
    {
        private const double ThumbW = 200;
        private const double ThumbH = 200;
        
        private readonly MenherachanContext _db;
        private readonly IBoardCollection _collection;
        private readonly IWebHostEnvironment _env;

        [HttpPost]
        public async Task<IActionResult> AddThread(Post post, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                var thread = new Thread
                {
                    AnonName = post.AnonName,
                    BoardId = post.BoardId,
                    OpIpHash = post.AnonIpHash,
                    IsClosed = false,
                };

                _db.Thread.Add(thread);
                _db.SaveChanges();

                var addPost = new Post
                {
                    ThreadId = thread.ThreadId,
                    AnonName = post.AnonName,
                    AnonIpHash = post.AnonIpHash,
                    BoardId = post.BoardId,
                    Subject = post.Subject,
                    TimeInUnixSeconds = DateTimeOffset.Now.ToUnixTimeSeconds(),
                    Comment = post.Comment,
                    Email = post.Email
                };

                _db.Post.Add(addPost);
                _db.SaveChanges();

                if (files != null && files.Count > 0)
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

                            if (extension == ".jpeg" || extension == ".jpg")
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
                                    
                                    var coefficient = image.Width > image.Height ? ThumbW / image.Width : ThumbH / image.Height;
                                    var thumbnailHeight = (int)Math.Round(image.Height * coefficient, MidpointRounding.ToEven);
                                    var thumbnailWidth = (int)Math.Round(image.Width * coefficient, MidpointRounding.ToEven);

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
        public IActionResult Board(string prefix)
        {
            ViewBag.Board = _collection.Boards.First(brd => brd.Prefix == prefix);
            ViewBag.UserIp =
                new MD5CryptoServiceProvider().ComputeHash(HttpContext.Connection.RemoteIpAddress.GetAddressBytes())
                    .GetString();

            var threadPostPostsList = new List<ThreadPostLastThreePosts>();

            var threads = _db.Thread.Join(_db.Board.Where(b => b.Prefix == prefix),
                t => t.BoardId,
                b => b.BoardId,
                (thread, board) => new Thread
                {
                    ThreadId = thread.ThreadId,
                    BoardId = thread.BoardId,
                    IsClosed = thread.IsClosed,
                    OpIpHash = thread.OpIpHash,
                    AnonName = thread.AnonName
                }).ToList();

            foreach (var thread in threads)
            {
                var postList = _db.Thread.Where(t => t.ThreadId == thread.ThreadId).Join(_db.Post,
                    t => t.ThreadId,
                    p => p.ThreadId,
                    (th, pt) => new Post
                    {
                        BoardId = pt.BoardId,
                        ThreadId = pt.ThreadId,
                        PostId = pt.PostId,
                        AnonIpHash = pt.AnonIpHash,
                        AnonName = pt.AnonName,
                        Subject = pt.Subject,
                        Comment = pt.Comment,
                        Email = pt.Email,
                        IsPinned = pt.IsPinned
                    }).Take(1).ToList();

                if (postList.Count > 0)
                {
                    var firstPost = postList[0];

                    var firstPostFiles = _db.Post.Where(p => p.PostId == firstPost.PostId).Join(_db.File,
                        p => p.PostId,
                        f => f.PostId,
                        (post, file) => new File
                        {
                            FileId = file.FileId,
                            BoardId = file.BoardId,
                            ThreadId = file.ThreadId,
                            PostId = file.PostId,
                            FileName = file.FileName,
                            ThumbnailName = file.ThumbnailName,
                            Info = file.Info
                        }).ToList();

                    var lTp = _db.Thread.Where(t => t.ThreadId == thread.ThreadId).Join(_db.Post,
                        t => t.ThreadId,
                        p => p.ThreadId,
                        (th, pt) => new Post
                        {
                            BoardId = pt.BoardId,
                            ThreadId = pt.ThreadId,
                            PostId = pt.PostId,
                            AnonIpHash = pt.AnonIpHash,
                            AnonName = pt.AnonName,
                            Subject = pt.Subject,
                            Comment = pt.Comment,
                            Email = pt.Email,
                            IsPinned = pt.IsPinned
                        }).Take(3).ToArray().OrderByDescending(p => p.TimeInUnixSeconds).ToArray();

                    var lTpFiles = new Dictionary<Post, List<File>>();

                    foreach (var post in lTp)
                    {
                        lTpFiles.Add(post, _db.Post.Where(p => p.PostId == firstPost.PostId).Join(_db.File,
                            p => p.PostId,
                            f => f.PostId,
                            (post_, file) => new File
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

            ViewBag.ThreadPostPostsList = threadPostPostsList;
            
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
        }
    }
}