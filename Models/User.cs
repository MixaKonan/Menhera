using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Menhera.Models
{
    [Table("user")]
    public class User : IdentityUser
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }
        
        [Column("login")]
        public string Login { get; set; }
        
        [Column("password_hash")]
        public override string PasswordHash { get; set; }
        
        [Column("email")]
        public override string Email { get; set; }
        
        [Column("is_admin")]
        public bool IsAdmin { get; set; }
    }
}