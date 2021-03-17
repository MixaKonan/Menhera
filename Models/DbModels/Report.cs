using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Menhera.Models.DbModels
{
    public class Report
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
        
        [Column("reason")]
        public string Reason { get; set; }
        
        [Column("time")]
        public DateTime Time { get; set; }
        
        [Column("ip")]
        public string Ip { get; set; }
    }
}