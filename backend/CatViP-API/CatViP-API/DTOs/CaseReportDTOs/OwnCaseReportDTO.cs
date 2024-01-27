using System.ComponentModel.DataAnnotations.Schema;

namespace CatViP_API.DTOs.CaseReportDTOs
{
    public class OwnCaseReportDTO
    {
        public long Id { get; set; }

        public string Description { get; set; } = null!;

        public DateTime DateTime { get; set; }

        public string Address { get; set; } = null!;

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string? CatName { get; set; }

        public ICollection<CaseReportImageDTO> CaseReportImages { get; set; } = new List<CaseReportImageDTO>();
    }
}
