namespace CatViP_API.DTOs.PostDTOs
{
    public class CommentDTO
    {
        public long Id { get; set; }

        public string Description { get; set; } = null!;

        public DateTime DateTime { get; set; }

        public string Username { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public bool IsCurrentLoginUser { get; set; }

        public byte[]? ProfileImage { get; set; }
    }
}
