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
using Menhera.Classes.Constants;
using Menhera.Classes.Db;
using Menhera.Classes.Files;
using Menhera.Classes.Logging;
using Menhera.Classes.Pagination;
using Menhera.Classes.PostFormatting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static Menhera.Classes.Logging.Logger;
using File = Menhera.Models.File;

namespace Menhera.Controllers
{
    public class BoardController : Controller
    {
        private readonly MenherachanContext _db;
        private readonly IBoardCollection _collection;
        private readonly IWebHostEnvironment _env;
        
        private readonly string _logDirectory;

        private readonly MD5CryptoServiceProvider _md5;

        public BoardController(MenherachanContext db, IWebHostEnvironment env, IBoardCollection collection)
        {
            _db = db;
            _env = env;
            _collection = collection;

            _logDirectory = Path.Combine(_env.WebRootPath, "logs", "board_logs.log");
            
            _md5 = new MD5CryptoServiceProvider();
        }

        [HttpPost]
        public async Task<IActionResult> AddThread(Post post, List<IFormFile> files)
        {
            try
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
                    var board = _db.Board.First(b => b.BoardId == post.BoardId);
                
                    if (_db.Thread.Count(t => t.BoardId == post.BoardId) >= 20)
                    {
                        return RedirectToAction("Board", new {prefix = board.Prefix});
                    }

                    post.AnonIpHash = ipHash;
                    post.Comment = PostFormatter.GetHtmlTrimmedComment(post);
                    
                    DbAccess.AddThreadToBoard(_db, ref post);

                    if (files.Count > 0)
                    {
                        var fileDirectory = Path.Combine(_env.WebRootPath, "postImages");

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
                    await LogIntoFile(_logDirectory, string.Concat("Added new thread: ", post.ThreadId),
                        LoggingInformationKind.Info);
                    return RedirectToAction("Thread", "Thread", new {id = post.ThreadId});
                }
            }
            catch (Exception e)
            {
                await LogIntoFile(_logDirectory, string.Concat(e.Message, "\n", e.StackTrace),
                    LoggingInformationKind.Error);
                Console.WriteLine(e);
                return StatusCode(500);
            }
            return RedirectToAction("Board");
        }

        [HttpGet]
        public async Task<IActionResult> Board(string prefix, int page = 1)
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
                    return NotFound();
                }

                ViewBag.ThreadCount = board.Thread.Count;

                foreach (var thrd in board.Thread.OrderByDescending(t => t.BumpInUnixTime))
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

                var pageInfo = new PageInfo(page, Constants.BOARD_PAGE_SIZE, allThreads.Count);

                var pageThreads = new List<KeyValuePair<Thread, List<KeyValuePair<Post, List<File>>>>>();

                for (var i = (page - 1) * pageInfo.PageSize; i < page * pageInfo.PageSize; i++)
                {
                    try
                    {
                        pageThreads.Add(allThreads[i]);
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        break;
                    }
                    catch (Exception e)
                    {
                        await LogIntoFile(_logDirectory, string.Concat(e.Message, "\n", e.StackTrace),
                            LoggingInformationKind.Error);
                        return StatusCode(500);
                    }
                }

                foreach (var pt in pageThreads)
                {
                    foreach (var pf in pt.Value)
                    {
                        pf.Key.Comment = PostFormatter.GetFormattedPostText(pf.Key);
                    }
                }

                ViewBag.PageInfo = pageInfo;

                ViewBag.BoardViewModel = pageThreads;
            }
            catch (InvalidOperationException e)
            {
                await LogIntoFile(_logDirectory, string.Concat(e.Message, "\n", e.StackTrace),
                    LoggingInformationKind.Error);
                return NotFound();
            }
            catch (Exception e)
            {
                await LogIntoFile(_logDirectory, string.Concat(e.Message, "\n", e.StackTrace),
                    LoggingInformationKind.Error);
                return StatusCode(500);
            }

            return View();
        }


        public async Task<IActionResult> Search(string prefix, string query = "")
        {
            try
            {
                var postsFiles = new Dictionary<Post, List<File>>();

                if (!string.IsNullOrWhiteSpace(query) || !string.IsNullOrEmpty(query))
                {
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            var boardId = _collection.Boards.First(brd => brd.Prefix == prefix).BoardId;

                            var posts = _db.Post.Where(p =>
                                    p.BoardId == boardId && (p.Comment.Contains(query) || p.Subject.Contains(query)))
                                .Include(p => p.File).Take(50).ToList();

                            foreach (var post in posts)
                            {
                                post.Comment = PostFormatter.GetFormattedPostText(post);
                                postsFiles.Add(post, post.File.ToList());
                            }
                        }
                        catch (InvalidOperationException e)
                        {
                            await LogIntoFile(_logDirectory, string.Concat(e.Message, "\n", e.StackTrace),
                                LoggingInformationKind.Error);
                            return NotFound();
                        }
                        catch (Exception e)
                        {
                            await LogIntoFile(_logDirectory, string.Concat(e.Message, "\n", e.StackTrace),
                                LoggingInformationKind.Error);
                            return StatusCode(500);
                        }
                    }
                }

                ViewBag.Board = _collection.Boards.First(brd => brd.Prefix == prefix);
                ViewBag.PostsFiles = postsFiles;
            }
            catch (InvalidOperationException e)
            {
                await LogIntoFile(_logDirectory, string.Concat(e.Message, "\n", e.StackTrace),
                    LoggingInformationKind.Error);
                return NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

            return View();
        }
    }
}