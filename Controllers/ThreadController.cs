using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Menhera.Classes.Anon;
using Menhera.Classes.Constants;
using Menhera.Classes.Db;
using Menhera.Classes.Files;
using Menhera.Classes.PostFormatting;
using Menhera.Database;
using Menhera.Extensions;
using Menhera.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using File = Menhera.Models.File;

namespace Menhera.Controllers
{
    public class ThreadController : Controller
    {
        private readonly MenherachanContext _db;
        private readonly IWebHostEnvironment _env;
        
        private readonly MD5CryptoServiceProvider _md5;

        private readonly Regex _lineBreakRegex;
        private readonly Regex _postReferenceRegex;
        
        
        public ThreadController(MenherachanContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;

            _md5 = new MD5CryptoServiceProvider();
            
            _lineBreakRegex = new Regex(Constants.HTML_POST_END_LINE_BREAK_PATTERN, RegexOptions.Compiled);
            _postReferenceRegex = new Regex(Constants.HTML_POST_REFERENCE_PATTERN, RegexOptions.Compiled);
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
                            post.Comment = PostFormatter.GetFormattedPostText(post);
                            
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
        public IActionResult AddPost(Post post, List<IFormFile> files)
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

                post.AnonIpHash = ipHash;
                post.TimeInUnixSeconds = DateTimeOffset.Now.ToUnixTimeSeconds();

                if (ModelState.IsValid)
                {
                    DbAccess.AddPostToThread(_db, post);

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
                }
                else
                {
                    RedirectToAction("Error", "Error", new {statusCode = 500});
                }
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