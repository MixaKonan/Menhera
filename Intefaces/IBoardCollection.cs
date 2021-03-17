using Menhera.Models;

namespace Menhera.Intefaces
{
    public interface IBoardCollection
    {
        Board[] Boards { get; }

        public Board this[int index]
        {
            get => Boards[index];
            set => Boards[index] = value;
        }
    }
}