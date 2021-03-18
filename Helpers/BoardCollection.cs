using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Menhera.Intefaces;
using Menhera.Models;

namespace Menhera.Helpers
{
    public class BoardCollection : IBoardCollection
    {
        public Board[] Boards { get; }
        public Dictionary<string, string> PrePostFixes { get; }
        public List<string> FileTypes { get; }

        public BoardCollection()
        {
            using (var sr = File.OpenText("wwwroot/boards.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                Boards = (Board[]) serializer.Deserialize(sr, typeof(Board[]));
            }

            PrePostFixes = new Dictionary<string, string>();
            FileTypes = new List<string> {"jpeg", "png", "gif", "webm"};
            

            if (Boards != null)
            {
                foreach (var board in Boards)
                {
                    PrePostFixes.Add(board.Prefix, board.Postfix);
                }
            }
        }
    }
}