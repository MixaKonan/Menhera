using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Menhera.Models
{
    [Table("post")]
    public class Post
    {
        [Key]
        [Column("post_id")]
        public int PostId { get; set; }

        [Column("board_id")]
        public int BoardId { get; set; }
        
        [Column("thread_id")]
        public int ThreadId { get; set; }
        
        [Column("email")]
        public string Email { get; set; }
        
        [Column("subject")]
        public string Subject { get; set; }
        
        [Column("comment")]
        public string Comment { get; set; }
        
        [Column("anon_name")]
        public string AnonName { get; set; }
        
        [Column("bump_in_unix_time")]
        public int BumpInUnixTime { get; set; }
        
        [Column("is_pinned")]
        public bool IsPinned { get; set; }
        
        [Column("time")]
        public DateTime Time { get; set; }
        
        [Column("anon_ip_hash")]
        public string AnonIpHash { get; set; }
        
    }
}