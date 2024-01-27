namespace CatViP_API.Models
{
    public class UserChat
    {
        public long Id { get; set; }

        public DateTime? LastSeen { get; set; }

        public long UserSendId { get; set; }

        public long UserReceiveId { get; set; }

        public virtual User UserReceive { get; set; } = null!;

        public virtual User UserSend { get; set; } = null!;

        public virtual ICollection<Chat> Chats { get; set; } = new List<Chat>();
    }
}
