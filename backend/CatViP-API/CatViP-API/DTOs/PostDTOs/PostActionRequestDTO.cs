using System.ComponentModel.DataAnnotations;

namespace CatViP_API.DTOs.PostDTOs
{
    public class PostActionRequestDTO
    {
        [Required]
        public long PostId { get; set; }
        [Required]
        [Range(1, 2, ErrorMessage = "ActionTypeId can only be 1 or 2.")]
        public long ActionTypeId { get; set; }
    }
}
