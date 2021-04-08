﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Menhera.Classes.Anon;
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

        public ThreadController(MenherachanContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;

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
                }
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }

            return View();
        }

        [HttpPost]
        public IActionResult AddPost(Post post, List<IFormFile> files, bool sage)
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

            return RedirectToAction("Thread", new {id = post.ThreadId});
        }
    }
}