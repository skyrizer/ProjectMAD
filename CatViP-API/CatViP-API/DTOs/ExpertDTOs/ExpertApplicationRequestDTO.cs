using System.ComponentModel.DataAnnotations;

namespace CatViP_API.DTOs.ExpertDTOs
{
    public class ExpertApplicationRequestDTO
    {
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public byte[] Documentation { get; set; } = null!;
    }
}
