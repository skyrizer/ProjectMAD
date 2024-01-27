using CatViP_API.DTOs.ChatDTOs;

namespace CatViP_API.Services.Interfaces
{
    public interface IChatService
    {
        ICollection<ChatDTO> GetChats(long authId, long userId);
        ICollection<ChatUserDTO> GetChatUsers(long authId);
        int GetUnreadChatsCount(long id);
        Task PushNotification(string sender, string receiver, string message);
        Task StoreChat(string sendUser, string receiveUser, string message);
        Task UpdateLastSeen(long authId, long userId);
    }
}
