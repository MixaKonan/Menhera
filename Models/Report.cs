using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Menhera.Models
{
    public partial class Report
    {
        public int ReportId { get; set; }
        public int BoardId { get; set; }
        public int ThreadId { get; set; }
        public int PostId { get; set; }
        public string Reason { get; set; }
        public long ReportTimeInUnixSeconds { get; set; }

        public virtual Board Board { get; set; }
        public virtual Thread Thread { get; set; }
    }
}
