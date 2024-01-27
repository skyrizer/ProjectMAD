namespace CatViP_API.Models
{
    public class CatCaseReportStatus
    {
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public virtual ICollection<CatCaseReport> CatCaseReports { get; set; } = new List<CatCaseReport>();
    }
}
