using System;
using System.Collections.Generic;
using Menhera.Models;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Menhera.Models
{
    public partial class Ban
    {
        public int BanId { get; set; }
        public int BoardId { get; set; }
        public int AdminId { get; set; }
        public string AnonIpHash { get; set; }
        public DateTime Time { get; set; }
        public string Reason { get; set; }

        public virtual Admin Admin { get; set; }
        public virtual Board Board { get; set; }
    }
}
