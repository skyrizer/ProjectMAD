using System.ComponentModel.DataAnnotations;

namespace CatViP_API.DTOs.PostDTOs
{
    public class PostReportRequestDTO
    {
        [Required]
        public long PostId { get; set; }
        [Required]
        public string Description { get; set; } = null!;
    }
}
