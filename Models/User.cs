using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Menhera.Models
{
    [Table("user")]
    public class User
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }
        
        [Column("login")]
        public string Login { get; set; }
        
        [Column("password_hash")]
        public string PasswordHash { get; set; }
        
        [Column("email")]
        public string Email { get; set; }
        
        [Column("is_admin")]
        public bool IsAdmin { get; set; }
    }
}