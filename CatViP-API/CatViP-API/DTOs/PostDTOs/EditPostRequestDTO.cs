using System.ComponentModel.DataAnnotations;

namespace CatViP_API.DTOs.PostDTOs
{
    public class EditPostRequestDTO
    {
        [Required]
        public string Description { get; set; } = null!;
    }
}
