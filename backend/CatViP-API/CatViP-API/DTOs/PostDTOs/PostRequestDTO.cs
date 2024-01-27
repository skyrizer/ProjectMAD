using System.ComponentModel.DataAnnotations;

namespace CatViP_API.DTOs.PostDTOs
{
    public class PostRequestDTO
    {
        [Required]
        public long PostTypeId { get; set; }
        [Required]
        public string Description { get; set; } = null!;
        public ICollection<PostImageDTO> PostImages { get; set; } = new List<PostImageDTO>();
        public ICollection<MentionedCatRequestDTO> MentionedCats { get; set; } = new List<MentionedCatRequestDTO>();
    }
}
