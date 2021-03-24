using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Menhera.Models
{
    public partial class Thread
    {
        public Thread()
        {
            File = new HashSet<File>();
            Post = new HashSet<Post>();
            Report = new HashSet<Report>();
        }

        public int ThreadId { get; set; }
        public int BoardId { get; set; }
        public bool IsClosed { get; set; }
        public string OpIpHash { get; set; }
        public string AnonName { get; set; }

        public virtual Board Board { get; set; }
        public virtual ICollection<File> File { get; set; }
        public virtual ICollection<Post> Post { get; set; }
        public virtual ICollection<Report> Report { get; set; }
    }
}
