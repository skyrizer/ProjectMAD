using System.ComponentModel.DataAnnotations;

namespace CatViP_API.DTOs.CaseReportDTOs
{
    public class CaseReportImageDTO
    {
        [Required]
        public byte[] Image { get; set; } = null!;
        [Required]
        public bool IsBloodyContent { get; set; }
    }
}
