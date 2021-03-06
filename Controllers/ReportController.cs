using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Menhera.Classes.Anon;
using Menhera.Classes.Logging;
using Menhera.Classes.PostFormatting;
using Menhera.Database;
using Menhera.Extensions;
using Menhera.Intefaces;
using Menhera.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Menhera.Classes.Logging.Logger;
using File = Menhera.Models.File;

namespace Menhera.Controllers
{
    public class ReportController : Controller
    {
        private readonly MenherachanContext _db;
        private readonly IBoardCollection _collection;
        private readonly IWebHostEnvironment _env;

        private readonly string _logDirectory;

        private readonly MD5CryptoServiceProvider _md5;

        public ReportController(MenherachanContext db, IBoardCollection collection, IWebHostEnvironment env)
        {
            _db = db;
            _collection = collection;
            _env = env;
            
            _logDirectory = Path.Combine(_env.WebRootPath, "logs", "report_logs.log");

            _md5 = new MD5CryptoServiceProvider();
        }

        [HttpGet]
        public async Task<IActionResult> Report(int postId)
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

            try
            {
                var post = _db.Post.Include(p => p.File)
                    .Include(p => p.Admin)
                    .First(p => p.PostId == postId) ?? throw new Exception();

                ViewBag.Board = _collection.Boards.First(brd => brd.BoardId == post.BoardId);

                ViewBag.Thread = _db.Thread.First(t => t.ThreadId == post.ThreadId);

                post.Comment = PostFormatter.GetFormattedPostText(post);

                ViewBag.PostFiles = new KeyValuePair<Post, List<File>>(post, post.File.ToList());

                return View();
            }
            catch (Exception e)
            {
                await LogIntoFile(_logDirectory, string.Concat(e.Message, "\n", e.StackTrace),
                    LoggingInformationKind.Error);
                return NotFound();
            }
        }

        [HttpPost]
        public async Task Report(int postId, string reason)
        {
            try
            {
                var post = _db.Post.First(p => p.PostId == postId);

                var report = new Report
                {
                    BoardId = post.BoardId,
                    ThreadId = post.ThreadId,
                    PostId = post.PostId,
                    Reason = reason,
                    ReportTimeInUnixSeconds = DateTimeOffset.Now.ToUnixTimeSeconds(),
                };

                _db.Report.Add(report);
                _db.SaveChanges();
                
                await LogIntoFile(_logDirectory, string.Concat
                        ("Added new report: ", report.PostId, "\t", report.Reason),
                    LoggingInformationKind.Info);
            }
            catch (Exception e)
            {
                await LogIntoFile(_logDirectory, string.Concat(e.Message, "\n", e.StackTrace),
                    LoggingInformationKind.Error);
                Console.WriteLine(e);
            }
           
        }
    }
}