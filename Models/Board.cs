using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Menhera.Models
{
    public partial class Board
    {
        public Board()
        {
            File = new HashSet<File>();
            Post = new HashSet<Post>();
            Report = new HashSet<Report>();
            Thread = new HashSet<Thread>();
        }

        public override string ToString()
        {
            return $"/{this.Prefix}/{this.Postfix}";
        }

        public int BoardId { get; set; }
        public string Prefix { get; set; }
        public string Postfix { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsHidden { get; set; }
        public bool AnonHasNoName { get; set; }
        public bool HasSubject { get; set; }
        public bool FilesAreAllowed { get; set; }
        public short FileLimit { get; set; }
        public string AnonName { get; set; }

        public virtual ICollection<File> File { get; set; }
        public virtual ICollection<Post> Post { get; set; }
        public virtual ICollection<Report> Report { get; set; }
        public virtual ICollection<Thread> Thread { get; set; }
    }
}
