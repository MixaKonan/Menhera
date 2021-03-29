using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
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

        [HttpPost]
        public void BanAnon(string anonIpHash, string reason, DateTime banEndTime)
        {
            var banEnd = new DateTimeOffset(banEndTime).ToUnixTimeSeconds();

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