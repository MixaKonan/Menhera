using System.Collections.Generic;
using Menhera.Models;

namespace Menhera.Intefaces
{
    public interface IBoardCollection
    {
        Board[] Boards { get; }
        public Dictionary<string, string> PrePostFixes { get; }
        public List<string> FileTypes { get; }

        public Board this[int index]
        {
            get => Boards[index];
            set => Boards[index] = value;
        }
    }
}