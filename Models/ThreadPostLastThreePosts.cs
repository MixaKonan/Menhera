namespace Menhera.Models
{
    public class ThreadPostLastThreePosts
    {
        public Thread Thread { get; set; }
        public Post Post { get; set; }
        public Post[] LastThreePosts { get; set; }
    }
}