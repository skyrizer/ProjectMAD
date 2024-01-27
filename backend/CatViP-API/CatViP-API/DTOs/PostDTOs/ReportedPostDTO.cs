namespace CatViP_API.DTOs.PostDTOs
{
    public class ReportedPostDTO
    {
        public long Id { get; set; }

        public string Description { get; set; } = null!;

        public DateTime DateTime { get; set; }

        public long UserId { get; set; }

        public string Username { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public byte[]? ProfileImage { get; set; }

        public long PostTypeId { get; set; }

        public ICollection<PostImageDTO> PostImages { get; set; } = new List<PostImageDTO>();
    }
}
