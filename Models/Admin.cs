using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Menhera.Models
{
    public partial class Admin
    {
        public Admin()
        {
            Ban = new HashSet<Ban>();
        }

        public override string ToString()
        {
            return $"Login: {this.Login}, Email: {this.Email}";
        }

        public int AdminId { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool CanDeletePosts { get; set; }
        public bool CanCloseThreads { get; set; }
        public bool HasAccessToPanel { get; set; }
        public bool CanBanUsers { get; set; }
        public string NicknameColorCode { get; set; }

        public virtual ICollection<Ban> Ban { get; set; }
        public virtual ICollection<Post> Post { get; set; }
    }
}
