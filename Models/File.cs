namespace Menhera.Models
{
    public class File
    {
        public int FileId { get; set; }
        public int BoardId { get; set; }
        public int ThreadId { get; set; }
        public int PostId { get; set; }
        public string FileName { get; set; }
        public string ThumbnailName { get; set; }
        public string Info { get; set; }


        public virtual Board Board { get; set; }
        public virtual Post Post { get; set; }
        public virtual Thread Thread { get; set; }
    }
}
