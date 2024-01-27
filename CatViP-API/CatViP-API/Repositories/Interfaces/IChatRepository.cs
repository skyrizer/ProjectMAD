using CatViP_API.Models;

namespace CatViP_API.Repositories.Interfaces
{
    public interface IChatRepository
    {
        ICollection<Chat> GetChats(long authId, long userId);
        ICollection<User> GetChatUsers(long authId);
        Task StoreChat(string sendUser, string receiveUser, string message);
        Chat GetLastestChat(long authId, long userId);
        Task UpdateLastSeen(long authId, long userId);
        int GetUnreadChatCount(long authId, long userId);
        int GetUnreadChatsCount(long authId);
    }
}
