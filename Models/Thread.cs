using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Menhera.Models
{
    [Table("thread")]
    public class Thread
    {
        [Key]
        [Column("thread_id")]
        public int ThreadId { get; set; }
        
        [Column("board_id")]
        public int BoardId { get; set; }
        
        [Column("is_closed")]
        public string IsClosed { get; set; }
        
        [Column("op")]
        public string Op { get; set; }
        
        [Column("is_hidden")]
        public bool IsHidden { get; set; }
        
        [Column("anon_has_no_name")]
        public bool AnonHasNoName { get; set; }
        
        [Column("anon_name")]
        public string AnonName { get; set; }
    }
}