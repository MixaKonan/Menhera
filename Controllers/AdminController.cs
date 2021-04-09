using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Menhera.Database;
using Menhera.Extensions;
using Menhera.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Menhera.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly MenherachanContext _db;
        private readonly IWebHostEnvironment _env;

        public AdminController(MenherachanContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        [HttpGet]
        public IActionResult Panel()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Admins()
        {
            ViewBag.Id = 1;

            return View();
        }

        [HttpPost]
        public void AddAdmin(string email, string login, string passwordHash, string ipHash,
            bool canBanUsers, bool canCloseThreads, bool canDeletePosts, bool hasAccessToPanel)
        {
            var admin = new Admin
            {
                Email = email,
                Login = login,
                PasswordHash = passwordHash,
                AdminIpHash = ipHash,
                CanBanUsers = canBanUsers,
                CanCloseThreads = canCloseThreads,
                CanDeletePosts = canDeletePosts,
                HasAccessToPanel = hasAccessToPanel
            };

            _db.Admin.Add(admin);
            _db.SaveChanges();
        }

        [HttpPost]
        public void RemoveAdmin(int adminId)
        {
            var admin = _db.Admin.First(a => a.AdminId == adminId);
            _db.Admin.Remove(admin);
            _db.SaveChanges();
        }

        [HttpGet]
        public IActionResult Boards()
        {
            ViewBag.Id = 1;

            return View();
        }

        [HttpPost]
        public void AddBoard(string prefix, string postfix, string title, string description, short fileLimit, string anonName,
            bool isHidden, bool anonHasNoName, bool hasSubject, bool filesAreAllowed)
        {
            var board = new Board
            {
                Prefix = prefix,
                Postfix = postfix,
                Title = title,
                Description = description,
                FileLimit = fileLimit,
                AnonName = anonName,
                IsHidden = isHidden,
                AnonHasNoName = anonHasNoName,
                HasSubject = hasSubject,
                FilesAreAllowed = filesAreAllowed
            };

            _db.Board.Add(board);
            _db.SaveChanges();
        }

        [HttpPost]
        public void RemoveBoard(int boardId)
        {
            var board = _db.Board.First(b => b.BoardId == boardId);
            
            var posts = _db.Post.Include(p => p.File).Where(p => p.BoardId == board.BoardId).ToList();
            
            _db.Board.Remove(board);
            
            foreach (var post in posts)     
            {
                foreach (var file in post.File)
                {
                    System.IO.File.Delete(Path.Combine(_env.WebRootPath, "postImages" ,file.FileName));
                    System.IO.File.Delete(Path.Combine(_env.WebRootPath, "thumbnails" ,file.ThumbnailName));
                }
            }
            
            _db.SaveChanges();
        }

        [HttpGet]
        public IActionResult Threads()
        {
            ViewBag.Threads = _db.Thread.Include(t => t.Board).Include(t => t.Post).ToList();

            return View();
        }

        [HttpPost]
        public void RemoveThread(int threadId)
        {
            var thread = _db.Thread.First(t => t.ThreadId == threadId);

            var posts = _db.Post.Include(p => p.File).Where(p => p.ThreadId == thread.ThreadId).ToList();

            _db.Thread.Remove(thread);
            
            foreach (var post in posts)     
            {
                foreach (var file in post.File)
                {
                    System.IO.File.Delete(Path.Combine(_env.WebRootPath, "postImages" ,file.FileName));
                    System.IO.File.Delete(Path.Combine(_env.WebRootPath, "thumbnails" ,file.ThumbnailName));
                }
            }
            
            _db.SaveChanges();
        }

        [HttpPost]
        public void CloseThread(int threadId)
        {
            var thread = _db.Thread.First(t => t.ThreadId == threadId);
            thread.IsClosed = true;

            _db.SaveChanges();
        }


        [HttpGet]
        public IActionResult Reports()
        {
            ViewBag.Reports = _db.Report.Include(r => r.Board).ToList();

            return View();
        }

        [HttpPost]
        public void RemoveReport(int reportId)
        {
            var report = _db.Report.First(r => r.ReportId == reportId);

            _db.Remove(report);
            _db.SaveChanges();
        }

        [HttpPost]
        public void DeletePost(int postId)
        {
            var post = _db.Post.Include(p => p.File).First(p => p.PostId == postId);
            
            _db.Post.Remove(post);
            
            if (post.File.Count > 0)
            {
                foreach (var file in post.File)
                {
                    System.IO.File.Delete(Path.Combine(_env.WebRootPath, "postImages" ,file.FileName));
                    System.IO.File.Delete(Path.Combine(_env.WebRootPath, "thumbnails" ,file.ThumbnailName));
                }
            }
            
            _db.SaveChanges();
        }

        [HttpPost]
        public void BanAnon(string anonIpHash, string reason, string banEndTime)
        {
            var bans = _db.Ban.Select(b => b).Where(b => b.AnonIpHash == anonIpHash).ToList();

            if (bans.Count > 0)
            {
                foreach (var ban in bans)
                {
                    _db.Ban.Remove(ban);
                }
            }

            var banEnd = ((DateTimeOffset) DateTime.Parse(banEndTime)).ToUnixTimeSeconds();

            var adminIpHash = new MD5CryptoServiceProvider().ComputeHash
                    (HttpContext.Connection.RemoteIpAddress.GetAddressBytes())
                .GetString();

            using (_db)
            {
                var adminId = _db.Admin.First(a => a.AdminIpHash == adminIpHash).AdminId;
                var banTime = DateTimeOffset.Now.ToUnixTimeSeconds();
                var ban = new Ban
                {
                    AdminId = adminId,
                    AnonIpHash = anonIpHash,
                    BanTimeInUnixSeconds = banTime,
                    Term = banEnd,
                    Reason = reason
                };

                _db.Ban.Add(ban);
                _db.SaveChanges();
            }
        }
    }
}