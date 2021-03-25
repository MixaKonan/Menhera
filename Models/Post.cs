using System;
using System.Collections.Generic;
using Menhera.Classes;

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
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Comment { get; set; }
        public string AnonName { get; set; }
        public long BumpInUnixTime { get; set; }
        public bool IsPinned { get; set; }
        public DateTime Time { get; set; }
        public string AnonIpHash { get; set; }

        public virtual Board Board { get; set; }
        public virtual Thread Thread { get; set; }
        public virtual ICollection<File> File { get; set; }
    }
}