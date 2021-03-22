using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Menhera.Models
{
    [Table("report")]
    public class Report
    {
        [Key]
        [Column("report_id")]
        public int ReportId { get; set; }
        
        [Column("board_id")]
        public int BoardId { get; set; }
        
        [Column("thread_id")]
        public int ThreadId { get; set; }
        
        [Column("post_id")]
        public int PostId { get; set; }
        
        [Column("reason")]
        public string Reason { get; set; }
        
        [Column("time")]
        public DateTime Time { get; set; } 
    }
}