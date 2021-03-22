using System.Collections.Generic;
using System.Linq;
using Menhera.Database;
using Menhera.Intefaces;
using Board = Menhera.Models.Board;

namespace Menhera.Singletons
{
    public class BoardCollection : IBoardCollection
    {
        public IEnumerable<Board> Boards { get; }
        public IEnumerable<string> FileTypes { get; }
        public Dictionary<string, string> PrePostFixes { get; } = new Dictionary<string, string>();

        private readonly MenherachanContext _db = new MenherachanContext();

        public BoardCollection()
        {
            Boards = _db.Boards.Select(board => board).ToList();
            FileTypes = new List<string> {"jpeg", "png", "gif", "webm"};

            foreach (var board in Boards)
            {
                PrePostFixes.Add(board.Prefix, board.Postfix);
            }
        }
    }
}