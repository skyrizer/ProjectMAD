namespace CatViP_API.DTOs.ChatDTOs
{
    public class ChatUserDTO
    {
        public long Id { get; set; }

        public string Username { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string LastestChat { get; set; } = null!;

        public int UnreadMessageCount { get; set; }

        public byte[]? ProfileImage { get; set; }
    }
}
