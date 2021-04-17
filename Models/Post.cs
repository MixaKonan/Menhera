using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Menhera.Models
{
    public partial class Post
    {
        public Post()
        {
            File = new HashSet<File>();
        }

        public int PostId { get; set; }
        public int BoardId { get; set; }
        public int ThreadId { get; set; }
        public int? AdminId { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Comment { get; set; }
        public string AnonName { get; set; }
        public bool IsPinned { get; set; }
        public long TimeInUnixSeconds { get; set; }
        public string AnonIpHash { get; set; }

        public virtual Board Board { get; set; }
        public virtual Thread Thread { get; set; }
        
        public virtual Admin Admin { get; set; }
        public virtual ICollection<File> File { get; set; }
    }
}
