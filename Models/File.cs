using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Menhera.Models
{
    [Table("file")]
    public class File
    {
        [Key]
        [Column("file_id")]
        public int FileId { get; set; }
        
        [Column("board_id")]
        public int BoardId { get; set; }
        
        [Column("thread_id")]
        public int ThreadId { get; set; }
        
        [Column("post_id")]
        public int PostId { get; set; }
        
        [Column("file_name")]
        public string FileName { get; set; }
        
        [Column("thumbnail_name")]
        public string ThumbnailName { get; set; }
        
        [Column("info")]
        public string Info { get; set; }
        
    }
}