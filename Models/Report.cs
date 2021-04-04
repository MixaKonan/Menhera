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
