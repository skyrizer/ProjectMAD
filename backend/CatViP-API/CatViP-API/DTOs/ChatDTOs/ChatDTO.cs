namespace CatViP_API.DTOs.ChatDTOs
{
    public class ChatDTO
    {
        public string Message { get; set; } = null!;

        public DateTime DateTime { get; set; }

        public bool IsCurrentUserSent { get; set; }
    }
}
