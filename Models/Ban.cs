using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Menhera.Models
{
    [Table("ban")]
    public class Ban
    {
        [Key]
        [Column("ban_id")]
        public int BanId { get; set; }

        [Column("board_id")]
        public int BoardId { get; set; }

        [Column("admin_id")]
        public int AdminId { get; set; }

        [Column("anon_ip_hash")]
        public string AnonIpHash { get; set; }

        [Column("time")]
        public DateTime Time { get; set; }

        [Column("reason")]
        public string Reason { get; set; }
    }
}