namespace CatViP_API.DTOs.CaseReportDTOs
{
    public class NearByCaseReportDTO
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public string Username { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public byte[]? ProfileImage { get; set; }

        public string Description { get; set; } = null!;

        public string Address { get; set; } = null!;

        public DateTime DateTime { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string? CatName { get; set; }

        public ICollection<CaseReportImageDTO> CaseReportImages { get; set; } = new List<CaseReportImageDTO>();
    }
}
