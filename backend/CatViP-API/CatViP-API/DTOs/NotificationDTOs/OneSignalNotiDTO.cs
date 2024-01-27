namespace CatViP_API.DTOs.NotificationDTOs
{
    public class OneSignalNotiDTO
    {
        public string app_id { get; set; } = null!;
        public List<string> include_external_user_ids { get; set; } = new List<string>();
        public Dictionary<string, string> contents { get; set; } = new Dictionary<string, string>();
    }
}
