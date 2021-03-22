using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Menhera.Models
{
    [Table("admin")]
    public class Admin
    {
        [Key]
        [Column("admin_id")]
        public int AdminId { get; set; }
        
        [Column("user_id")]
        public int UserId { get; set; }
        
        [Column("is_superadmin")]
        public bool IsSuperadmin { get; set; }

    }
        
}