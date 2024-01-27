using System.ComponentModel.DataAnnotations;

namespace CatViP_API.DTOs.ExpertDTOs
{
    public class ExpertApplicationActionRequestDTO
    {
        [Required]
        public long ApplictionId { get; set; }
        [Required]
        public long StatusId { get; set; }
        public string? RejectedReason { get; set; }
    }
}
