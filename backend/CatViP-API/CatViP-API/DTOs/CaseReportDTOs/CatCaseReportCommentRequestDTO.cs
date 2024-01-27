using System.ComponentModel.DataAnnotations;

namespace CatViP_API.DTOs.CaseReportDTOs
{
    public class CatCaseReportCommentRequestDTO
    {
        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public long CatCaseReportId { get; set; }
    }
}
