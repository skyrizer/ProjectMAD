namespace CatViP_API.DTOs.PostDTOs
{
    public class PostDTO
    {
        public long Id { get; set; }

        public string Description { get; set; } = null!;

        public DateTime? DateTime { get; set; }

        public decimal? Price { get; set; }

        public string? AdsUrl { get; set; }

        public long UserId { get; set; }

        public bool? IsCurrentUserPost { get; set; }

        public string Username { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public byte[]? ProfileImage { get; set; }

        public long? PostTypeId { get; set; }

        public int? LikeCount { get; set; }

        public int? CurrentUserAction { get; set; }

        public int? CommentCount { get; set; }

        public int? DislikeCount { get; set; }

        public bool IsAds {  get; set; } = false;

        public ICollection<PostImageDTO> PostImages { get; set; } = new List<PostImageDTO>();

        public ICollection<MentionedCatDTO> MentionedCats { get; set; } = new List<MentionedCatDTO>();
    }
}
