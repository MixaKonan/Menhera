using System;
using System.Linq;
using Menhera.Database;
using Menhera.Models;
using Microsoft.EntityFrameworkCore;

namespace Menhera.Classes
{
    public class BoardInformation
    {
        public int ThreadCount { get; }
        public int PostCount { get; }
        public int FileCount { get; }

        public BoardInformation(Board board, MenherachanContext db)
        {
            ThreadCount = db.Board.Where(b => b.Prefix == board.Prefix)
                .Join(db.Thread,
                b => b.BoardId,
                t => t.BoardId,
                (br, thread) => thread.ThreadId).Count();
            
            PostCount = db.Board.Where(b => b.Prefix == board.Prefix).Join(db.Post,
                b => b.BoardId,
                p => p.BoardId,
                (brd, post) => post.PostId).Count();
            
            FileCount = db.Board.Join(db.File,
                b => b.BoardId,
                f => f.BoardId,
                (br, fl) => fl.FileId).Count();
        }
    }
}