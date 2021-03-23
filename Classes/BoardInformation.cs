using System;
using System.Collections;
using System.Linq;
using Menhera.Database;
using Menhera.Models;

namespace Menhera.Classes
{
    public class BoardInformation
    {
        public int ThreadCount { get; }
        public int PostCount { get; }
        public int PostsPerHour { get; }
        
        public BoardInformation(Board board, MenherachanContext db)
        {
            ThreadCount = db.Boards.Where(b => b.Prefix == board.Prefix)
                .Join(db.Threads,
                b => b.BoardId,
                t => t.BoardId,
                (br, thread) => thread.ThreadId).Count();

            PostCount = db.Boards.Where(b => b.Prefix == board.Prefix).Join(db.Posts,
                b => b.BoardId,
                p => p.BoardId,
                (brd, post) => post.PostId).Count();

            PostsPerHour = db.Threads.Join(db.Posts.Where(p => (DateTime.Now - p.Time).TotalHours <= 1),
                t => t.ThreadId,
                p => p.ThreadId,
                (th, pt) => pt.PostId).Count();
        }
    }
}