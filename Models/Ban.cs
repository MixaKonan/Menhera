using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Menhera.Models
{
    public partial class Ban
    {
        public override string ToString()
        {
            return $"Admin: {this.Admin.Login}. Ban time: {this.BanTimeInUnixSeconds}.";
        }

        public int BanId { get; set; }
        public int AdminId { get; set; }
        public string AnonIpHash { get; set; }
        public long BanTimeInUnixSeconds { get; set; }
        public long Term { get; set; }
        public string Reason { get; set; }

        public virtual Admin Admin { get; set; }
    }
}
