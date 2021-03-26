using System.Collections.Generic;

namespace Menhera.Models
{
    public class ThreadPostLastThreePosts
    {
        public Thread Thread { get; set; }
        public KeyValuePair<Post, List<File>> FirstPost { get; set; }
        public Dictionary<Post, List<File>> LastThreePosts { get; set; }
    }
}