using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Menhera.Classes.Logging;
using Menhera.Database;
using Menhera.Extensions;
using Menhera.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Menhera.Classes.Logging.Logger;

namespace Menhera.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly MenherachanContext _db;
        private readonly IWebHostEnvironment _env;

        private readonly string _logDirectory;
        private readonly string _errorLogDirectory;

        public AdminController(MenherachanContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;

            _logDirectory = Path.Combine(_env.WebRootPath, "logs", "admin_logs.log");
            _errorLogDirectory = Path.Combine(_env.WebRootPath, "logs", "admin_error_logs.log");
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
        public async Task<IActionResult> AddAdminAsync(string email, string login, string passwordHash, string ipHash,
            bool canBanUsers, bool canCloseThreads, bool canDeletePosts, bool hasAccessToPanel)
        {
            try
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

                await LogIntoFile(_logDirectory, string.Concat("Added new admin", admin.ToString()),
                    LoggingInformationKind.Info);
            }
            catch (Exception e)
            {
                await LogIntoFile(_errorLogDirectory, string.Concat(e.Message, "\n", e.StackTrace),
                    LoggingInformationKind.Error);
                Console.WriteLine(e);
                return StatusCode(500);
            }

            return StatusCode(200);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveAdminAsync(int adminId)
        {
            try
            {
                var admin = _db.Admin.First(a => a.AdminId == adminId);
                var _admin = new Admin
                {
                    Login = admin.Login,
                    Email = admin.Email
                };
                _db.Admin.Remove(admin);
                _db.SaveChanges();

                await LogIntoFile(_logDirectory, string.Concat("Removed admin. ", _admin.ToString()),
                    LoggingInformationKind.Info);
            }
            catch (Exception e)
            {
                await LogIntoFile(_errorLogDirectory, string.Concat(e.Message, "\n", e.StackTrace),
                    LoggingInformationKind.Error);
                Console.WriteLine(e);
                return StatusCode(500);
            }

            return StatusCode(200);
        }

        [HttpGet]
        public IActionResult Boards()
        {
            ViewBag.Id = 1;

            return View();
        }

        [HttpPost]
        public async void AddBoardAsync(string prefix, string postfix, string title, string description,
            short fileLimit,
            string anonName,
            bool isHidden, bool anonHasNoName, bool hasSubject, bool filesAreAllowed)
        {
            try
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

                await LogIntoFile(_logDirectory, string.Concat("Added new board. ", board.ToString()),
                    LoggingInformationKind.Info);
            }
            catch (Exception e)
            {
                await LogIntoFile(_errorLogDirectory, string.Concat(e.Message, "\n", e.StackTrace),
                    LoggingInformationKind.Error);
                Console.WriteLine(e);
            }
        }

        [HttpPost]
        public async void RemoveBoardAsync(int boardId)
        {
            try
            {
                var board = _db.Board.First(b => b.BoardId == boardId);
                var _board = new Board {Prefix = board.Prefix, Postfix = board.Postfix};

                var posts = _db.Post.Include(p => p.File).Where(p => p.BoardId == board.BoardId).ToList();

                _db.Board.Remove(board);

                foreach (var post in posts)
                {
                    foreach (var file in post.File)
                    {
                        System.IO.File.Delete(Path.Combine(_env.WebRootPath, "postImages", file.FileName));
                        System.IO.File.Delete(Path.Combine(_env.WebRootPath, "thumbnails", file.ThumbnailName));
                    }
                }

                _db.SaveChanges();

                await LogIntoFile(_logDirectory, string.Concat("Removed board. ", _board.ToString()),
                    LoggingInformationKind.Info);
            }
            catch (Exception e)
            {
                await LogIntoFile(_errorLogDirectory, string.Concat(e.Message, "\n", e.StackTrace),
                    LoggingInformationKind.Error);
                Console.WriteLine(e);
            }
        }

        [HttpGet]
        public IActionResult Threads()
        {
            ViewBag.Threads = _db.Thread.Include(t => t.Board).Include(t => t.Post).ToList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveThreadAsync(int threadId)
        {
            try
            {
                var thread = _db.Thread.First(t => t.ThreadId == threadId);

                var posts = _db.Post.Include(p => p.File).Where(p => p.ThreadId == thread.ThreadId).ToList();

                if (posts.Count > 0)
                {
                    _db.Thread.Remove(thread);
                }

                foreach (var post in posts)
                {
                    foreach (var file in post.File)
                    {
                        System.IO.File.Delete(Path.Combine(_env.WebRootPath, "postImages", file.FileName));
                        System.IO.File.Delete(Path.Combine(_env.WebRootPath, "thumbnails", file.ThumbnailName));
                    }
                }

                _db.SaveChanges();

                await LogIntoFile(_logDirectory, string.Concat("Removed thread: ", threadId),
                    LoggingInformationKind.Info);
            }
            catch (Exception e)
            {
                await LogIntoFile(_errorLogDirectory, string.Concat(e.Message, "\n", e.StackTrace),
                    LoggingInformationKind.Error);
                Console.WriteLine(e);
                return StatusCode(500);
            }

            return StatusCode(200);
        }

        [HttpPost]
        public async Task<IActionResult> CloseThreadAsyncAsync(int threadId)
        {
            try
            {
                var thread = _db.Thread.First(t => t.ThreadId == threadId);
                thread.IsClosed = true;

                _db.SaveChanges();

                await LogIntoFile(_logDirectory, string.Concat("Closed thread: ", threadId),
                    LoggingInformationKind.Info);
            }
            catch (Exception e)
            {
                await LogIntoFile(_errorLogDirectory, string.Concat(e.Message, "\n", e.StackTrace),
                    LoggingInformationKind.Error);
                Console.WriteLine(e);
                return StatusCode(500);
            }

            return StatusCode(200);
        }


        [HttpGet]
        public IActionResult Reports()
        {
            ViewBag.Reports = _db.Report.Include(r => r.Board).ToList();

            return View();
        }

        [HttpPost]
        public async void RemoveReportAsync(int reportId)
        {
            try
            {
                var report = _db.Report.First(r => r.ReportId == reportId);

                _db.Remove(report);
                _db.SaveChanges();

                await LogIntoFile(_logDirectory, string.Concat("Removed report: ", report),
                    LoggingInformationKind.Info);
            }
            catch (Exception e)
            {
                await LogIntoFile(_errorLogDirectory, string.Concat(e.Message, "\n", e.StackTrace),
                    LoggingInformationKind.Error);
                Console.WriteLine(e);
            }
        }

        [HttpPost]
        public async void DeletePostAsync(int postId)
        {
            try
            {
                var post = _db.Post.Include(p => p.File).First(p => p.PostId == postId);

                _db.Post.Remove(post);

                if (post.File.Count > 0)
                {
                    foreach (var file in post.File)
                    {
                        System.IO.File.Delete(Path.Combine(_env.WebRootPath, "postImages", file.FileName));
                        System.IO.File.Delete(Path.Combine(_env.WebRootPath, "thumbnails", file.ThumbnailName));
                    }
                }

                _db.SaveChanges();

                await LogIntoFile(_logDirectory, string.Concat("Removed post: ", postId), LoggingInformationKind.Info);
            }
            catch (Exception e)
            {
                await LogIntoFile(_errorLogDirectory, string.Concat(e.Message, "\n", e.StackTrace),
                    LoggingInformationKind.Error);
                Console.WriteLine(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> BanAnonAsync(string anonIpHash, string reason, string banEndTime)
        {
            try
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

                    await LogIntoFile(_logDirectory, string.Concat("Added new ban: ", ban),
                        LoggingInformationKind.Info);
                }
            }
            catch (Exception e)
            {
                await LogIntoFile(_errorLogDirectory, string.Concat(e.Message, "\n", e.StackTrace),
                    LoggingInformationKind.Error);
                Console.WriteLine(e);
                return StatusCode(500);
            }

            return StatusCode(200);
        }
    }
}