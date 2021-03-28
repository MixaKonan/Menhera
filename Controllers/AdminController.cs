﻿using System;
using System.Linq;
using System.Security.Cryptography;
using Menhera.Database;
using Menhera.Extensions;
using Menhera.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Menhera.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly MenherachanContext _db;
        
        // GET
        public IActionResult Panel()
        {
            return View();
        }

        public IActionResult Create(BoardAdmin boardAdmin)
        {
            return View();
        }
        
        public IActionResult Admins()
        {
            return View();
        }
        
        public IActionResult Boards()
        {
            return View();
        }

        [HttpPost]
        public void DeletePost(int postId)
        {
            var post = new Post {PostId = postId};
            using (_db)
            {
                _db.Remove(post);
                _db.SaveChanges();
            }
        }

        [HttpGet]
        public IActionResult BanAnon(int postId)
        {
            Post post;

            using (_db)
            {
                ViewBag.ThreadOpIpHash = _db.Thread.Join(_db.Post.Where(p => p.PostId == postId),
                    t => t.ThreadId,
                    p => p.ThreadId,
                    (t, p) => new Thread
                    {
                        OpIpHash = t.OpIpHash
                    }).ToList()[0].OpIpHash;

                post = _db.Post.First(p => p.PostId == postId);

                ViewBag.PostFiles = _db.Post.Where(p => p.PostId == postId)
                    .Include(p => p.File).ToList()[0].File.ToList();
            }

            return View("Ban", post);
        }
        
        [HttpPost]
        public void BanAnon(string anonIpHash)
        {
            var adminIpHash = new MD5CryptoServiceProvider().ComputeHash
                    (HttpContext.Connection.RemoteIpAddress.GetAddressBytes())
                .GetString();

            using (_db)
            {
                var adminId = _db.Admin.First(a => a.AdminIpHash == adminIpHash).AdminId;
                var banTime = DateTimeOffset.Now.ToUnixTimeSeconds();
                var banEndTime = banTime + 3600; 
                var ban = new Ban
                {
                    AdminId = adminId,
                    AnonIpHash = anonIpHash,
                    BanTimeInUnixSeconds = banTime,
                    Term = banEndTime,
                    Reason = "Test"
                };

                _db.Ban.Add(ban);
                _db.SaveChanges();
            }

        }

        public AdminController(MenherachanContext db)
        {
            _db = db;
        }
    }
}