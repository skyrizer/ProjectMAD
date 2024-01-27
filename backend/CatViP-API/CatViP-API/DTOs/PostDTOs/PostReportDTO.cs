namespace CatViP_API.DTOs.PostDTOs
{
    public class PostReportDTO
    {
        public string Username { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public byte[]? ProfileImage { get; set; }

        public string Description { get; set; } = null!;

        public DateTime DateTime { get; set; }
    }
}
