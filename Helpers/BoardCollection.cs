using System.IO;
using Newtonsoft.Json;
using Menhera.Intefaces;
using Menhera.Models;

namespace Menhera.Helpers
{
    public class BoardCollection : IBoardCollection
    {
        public Board[] Boards { get; }

        public BoardCollection()
        {
            using(var sr = File.OpenText("wwwroot/boards.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                Boards = (Board[])serializer.Deserialize(sr, typeof(Board[]));
            }
        }
    }
}