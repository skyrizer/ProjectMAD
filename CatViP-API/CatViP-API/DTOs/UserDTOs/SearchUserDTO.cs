namespace CatViP_API.DTOs.UserDTOs
{
    public class SearchUserDTO
    {
        public long Id { get; set; }
        public string Username { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public byte[]? ProfileImage { get; set; }
    }
}
