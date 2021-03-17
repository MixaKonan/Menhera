using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Menhera.Models.DbModels
{
    public class File
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        [Column("board")]
        public string Board { get; set; }
        
        [Column("threadId")]
        public int ThreadId { get; set; }
        
        [Column("postId")]
        public int PostId { get; set; }
        
        [Column("origName")]
        public string OrigName { get; set; }

        [Column("origName")]
        public string PathName { get; set; }
        
        [Column("thumbName")]
        public string ThumbName { get; set; }
        
        [Column("info")]
        public string Info { get; set; }
    }
}