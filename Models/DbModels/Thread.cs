using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Menhera.Models.DbModels
{
    public class Thread
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        [Column("board")]
        public string Board { get; set; }
        
        [Column("parent")]
        public string Parent { get; set; }
        
        [Column("email")]
        public string Email { get; set; }
        
        [Column("name")]
        public string Name { get; set; }
        
        [Column("subject")]
        public string Subject { get; set; }
        
        [Column("comment")]
        public string Comment { get; set; }
        
        [Column("op")]
        public string ThreadCreator { get; set; }
        
        [Column("user")]
        public string User { get; set; }
        
        [Column("bump")]
        public int Bump { get; set; }
        
        [Column("sticked")]
        public int Sticked { get; set; }
        
        [Column("closed")]
        public int Closed { get; set; }
        
        [Column("password")]
        public int Password { get; set; }
        
        [Column("time")]
        public DateTime Time { get; set; }
        
        [Column("ip")]
        public string Ip { get; set; }            
    }
}