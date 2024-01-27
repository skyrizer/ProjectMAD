using System.ComponentModel.DataAnnotations;

namespace CatViP_API.DTOs.PostDTOs
{
    public class MentionedCatRequestDTO
    {
        [Required]
        public long CatId { get; set; }
    }
}
