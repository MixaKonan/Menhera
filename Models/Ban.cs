namespace Menhera.Models
{
    public partial class Ban
    {
        public int BanId { get; set; }
        public int BoardId { get; set; }
        public int AdminId { get; set; }
        public string AnonIpHash { get; set; }
        
        public long BanTimeInUnixSeconds { get; set; }
        public long Term { get; set; }
        public string Reason { get; set; }

        public virtual Admin Admin { get; set; }
        public virtual Board Board { get; set; }
    }
}
