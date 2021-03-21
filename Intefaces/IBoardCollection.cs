using System.Collections.Generic;
using Board = Menhera.Models.Board;

namespace Menhera.Intefaces
{
    public interface IBoardCollection
    {
        public IEnumerable<Board> Boards { get; }
        public IEnumerable<string> FileTypes { get; }
        public Dictionary<string, string> PrePostFixes { get; }
    }
}