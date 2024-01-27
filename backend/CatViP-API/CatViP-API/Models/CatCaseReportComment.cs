namespace CatViP_API.Models
{
    public class CatCaseReportComment
    {
        public long Id { get; set; }

        public string Description { get; set; } = null!;

        public DateTime DateTime { get; set; }

        public long CatCaseReportId { get; set; }

        public long UserId { get; set; }

        public virtual CatCaseReport CatCaseReport { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
