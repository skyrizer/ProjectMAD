using CatViP_API.DTOs.PostDTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatViP_API.DTOs.CaseReportDTOs
{
    public class CaseReportRequestDTO
    {
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public string Address { get; set; } = null!;
        [Required]
        public decimal Latitude { get; set; }
        [Required]  
        public decimal Longitude { get; set; }

        public long? CatId { get; set; }

        public ICollection<CaseReportImageDTO> CaseReportImages { get; set; } = new List<CaseReportImageDTO>();
    }
}
