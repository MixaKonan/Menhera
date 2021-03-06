using System.Collections.Generic;
using System.Linq;
using Menhera.Database;
using Menhera.Intefaces;
using Menhera.Models;

namespace Menhera.Transients
{
    public class BoardCollection : IBoardCollection
    {
        public IEnumerable<Board> Boards { get; }
        public Dictionary<string, string> PrePostFixes { get; } = new Dictionary<string, string>();

        private readonly MenherachanContext _db = new MenherachanContext();

        public BoardCollection()
        {
            Boards = _db.Board.Select(board => board).ToList();

            foreach (var board in Boards)
            {
                PrePostFixes.Add(board.Prefix, board.Postfix);
            }
        }
    }
}