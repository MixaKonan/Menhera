using System;
using System.Linq;
using System.Security.Cryptography;
using Menhera.Database;
using Menhera.Extensions;
using Menhera.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Menhera.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly MenherachanContext _db;
        
        
        public IActionResult Panel()
        {
            return View();
        }

        public IActionResult Create()
        {
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
        
        [HttpPost]
        public void AddBoard(string prefix, string postfix, string title, string description, short fileLimit , string anonName,
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
            _db.Board.Remove(board);
            _db.SaveChanges();
        }
        
        public IActionResult Admins()
        {
            ViewBag.Id = 1;
            
            return View();
        }
        
        public IActionResult Boards()
        {
            ViewBag.Id = 1;
            
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
            
            var banEnd = ((DateTimeOffset)DateTime.Parse(banEndTime)).ToUnixTimeSeconds();
            
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

        public AdminController(MenherachanContext db)
        {
            _db = db;
        }
    }
}