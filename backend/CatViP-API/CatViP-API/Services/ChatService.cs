using AutoMapper;
using CatViP_API.DTOs.ChatDTOs;
using CatViP_API.Helpers;
using CatViP_API.Models;
using CatViP_API.Repositories.Interfaces;
using CatViP_API.Services.Interfaces;

namespace CatViP_API.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ChatService(IChatRepository chatRepository, IUserRepository userRepository, IMapper mapper)
        {
            _chatRepository = chatRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public ICollection<ChatDTO> GetChats(long authId, long userId)
        {
            var chats = _chatRepository.GetChats(authId, userId);

            var chatDTOs = new List<ChatDTO>();

            foreach (var chat in chats)
            {
                chatDTOs.Add(new ChatDTO { Message = chat.Message, DateTime = chat.DateTime, IsCurrentUserSent = chat.UserChat.UserSendId == authId });
            }

            return chatDTOs;
        }

        public ICollection<ChatUserDTO> GetChatUsers(long authId)
        {
            var chatUsers = _chatRepository.GetChatUsers(authId);

            var chatUserDTOs = new List<ChatUserDTO>();

            foreach (var chatUser in chatUsers)
            {
                var lastestchat = _chatRepository.GetLastestChat(authId, chatUser.Id);

                var chatuserDTO = _mapper.Map<ChatUserDTO>(chatUser);
                chatuserDTO.LastestChat = ((lastestchat.UserChat.UserSendId == authId ? "You: " : lastestchat.UserChat.UserSend.Username + ": ") + lastestchat.Message);

                if (chatuserDTO.LastestChat.Length > 30)
                {
                    chatuserDTO.LastestChat = chatuserDTO.LastestChat.Substring(0, 27) + "...";
                }

                chatuserDTO.UnreadMessageCount = _chatRepository.GetUnreadChatCount(authId, chatUser.Id);

                chatUserDTOs.Add(chatuserDTO);
            }

            return chatUserDTOs;
        }

        public int GetUnreadChatsCount(long authId)
        {
            return _chatRepository.GetUnreadChatsCount(authId);
        }

        public async Task PushNotification(string sender, string receiver, string message)
        {
            var receiveUser = _userRepository.GetActiveCatOwnerOrExpertByUsername(receiver);
            var sendUser = _userRepository.GetActiveCatOwnerOrExpertByUsername(sender);

            if (receiveUser == null || sendUser == null)
            {
                return;
            }

            List<string> usernames = new List<string>() { receiveUser.Username!};

            await OneSignalSendNotiHelper.OneSignalSendChatNoti(usernames, sendUser.Username, message);
        }

        public async Task StoreChat(string sendUser, string receiveUser, string message)
        {
            await _chatRepository.StoreChat(sendUser, receiveUser, message);
        }

        public async Task UpdateLastSeen(long authId, long userId)
        {
            await _chatRepository.UpdateLastSeen(authId, userId);
        }
    }
}
