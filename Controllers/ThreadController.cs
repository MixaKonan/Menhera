using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Menhera.Classes.Anon;
using Menhera.Classes.Db;
using Menhera.Classes.Files;
using Menhera.Classes.Logging;
using Menhera.Classes.PostFormatting;
using Menhera.Database;
using Menhera.Extensions;
using Menhera.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Menhera.Classes.Logging.Logger;
using File = Menhera.Models.File;

namespace Menhera.Controllers
{
    public class ThreadController : Controller
    {
        private readonly MenherachanContext _db;
        private readonly IWebHostEnvironment _env;
        
        private readonly string _logDirectory;


        private readonly MD5CryptoServiceProvider _md5;

        public ThreadController(MenherachanContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;

            _logDirectory = Path.Combine(_env.WebRootPath, "logs", "thread_logs.log");

            _md5 = new MD5CryptoServiceProvider();
        }

        [HttpGet]
        public async Task<IActionResult> Thread(int id)
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
                        var thread = _db.Thread.Where(t => t.ThreadId == id).Include(t => t.Board).Include(t => t.Post)
                            .ToList()[0];

                        var posts = _db.Post.Where(p => p.ThreadId == id).Include(p => p.File).ToList();

                        var postsFiles = new Dictionary<Post, List<File>>();

                        foreach (var post in posts)
                        {
                            post.Comment = PostFormatter.GetFormattedPostText(post);

                            postsFiles.Add(post, post.File.ToList());
                        }

                        ViewBag.Board = thread.Board;
                        ViewBag.Thread = thread;
                        ViewBag.PostsFiles = postsFiles;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        return NotFound();
                    }
                    catch (Exception e)
                    {
                        await LogIntoFile(_logDirectory, string.Concat(e.Message, "\n", e.StackTrace),
                            LoggingInformationKind.Error);
                    }
                }
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                await LogIntoFile(_logDirectory, string.Concat(e.Message, "\n", e.StackTrace),
                    LoggingInformationKind.Error);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(Post post, List<IFormFile> files, bool sage)
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

                post.Comment = PostFormatter.GetHtmlTrimmedComment(post);
                post.AnonIpHash = ipHash;
                post.TimeInUnixSeconds = DateTimeOffset.Now.ToUnixTimeSeconds();

                if (ModelState.IsValid)
                {
                    DbAccess.AddPostToThread(_db, post, sage);

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
                                    creator.CreateThumbnail();

                                    DbAccess.AddFilesToPost(_db, post, creator.ImgInfo);
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("FileLengthNotValid", "Файл пустой.");
                            }
                        }
                    }
                    await LogIntoFile(_logDirectory, string.Concat("Added new post: ", post.PostId, "at thread: ", post.ThreadId),
                        LoggingInformationKind.Info);
                }
                else
                {
                    return StatusCode(500);
                }
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                await LogIntoFile(_logDirectory, string.Concat(e.Message, "\n", e.StackTrace),
                    LoggingInformationKind.Error);
            }

            return RedirectToAction("Thread", new {id = post.ThreadId});
        }

        public async Task<IActionResult> Search(int threadId, string query = "")
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
                            var posts = _db.Post.Where(p =>
                                    p.ThreadId == threadId && (p.Comment.Contains(query) || p.Subject.Contains(query)))
                                .Include(p => p.File).Take(50).ToList();

                            foreach (var post in posts)
                            {
                                post.Comment = PostFormatter.GetFormattedPostText(post);
                                postsFiles.Add(post, post.File.ToList());
                            }
                        }
                        catch (InvalidOperationException)
                        {
                            return NotFound();
                        }
                    }
                }

                ViewBag.Thread = _db.Thread.Include(t => t.Board).First(t => t.ThreadId == threadId);
                ViewBag.Board = ViewBag.Thread.Board;
                ViewBag.PostsFiles = postsFiles;
            }
            catch (InvalidOperationException)
            {
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
    }
}