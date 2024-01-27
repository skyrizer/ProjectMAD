using CatViP_API.Data;
using CatViP_API.Models;
using CatViP_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace CatViP_API.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly CatViPContext _context;

        public ChatRepository(CatViPContext context)
        {
            _context = context;
        }

        public ICollection<Chat> GetChats(long authId, long userId)
        {
            return _context.Chats
                .Include(x => x.UserChat)
                .Include(x => x.UserChat.UserSend).Include(x => x.UserChat.UserReceive)
                .Where(c => (c.UserChat.UserSendId == authId && c.UserChat.UserReceiveId == userId) || 
                            (c.UserChat.UserSendId == userId && c.UserChat.UserReceiveId == authId))
                .OrderByDescending(c => c.DateTime)
                .ToList();
        }

        public ICollection<User> GetChatUsers(long authId)
        {
            return _context.Chats
                .Include(x => x.UserChat.UserSend).Include(x => x.UserChat.UserReceive)
                .ToList()
                .Where(c => c.UserChat.UserSendId == authId || c.UserChat.UserReceiveId == authId)
                .Select(c => new {
                    User = c.UserChat.UserSendId == authId ? c.UserChat.UserReceive : c.UserChat.UserSend,
                    LastChatDate = c.DateTime
                })
                .GroupBy(x => x.User.Id)
                .Select(x => new {
                    User = x.First().User,
                    LastChatDate = x.Max(y => y.LastChatDate)
                })
                .OrderByDescending(x => x.LastChatDate)
                .Select(x => x.User)
                .ToList();
        }

        public Chat GetLastestChat(long authId, long userId)
        {
            return _context.Chats
                .Include(x => x.UserChat.UserSend).Include(x => x.UserChat.UserReceive)
                .ToList()
                .Where(c => (c.UserChat.UserSendId == authId && c.UserChat.UserReceiveId == userId) || (c.UserChat.UserSendId == userId && c.UserChat.UserReceiveId == authId))
                .OrderByDescending(x => x.DateTime)
                .First();
        }

        public int GetUnreadChatCount(long authId, long userId)
        {
            var authUserChat = _context.UserChats.FirstOrDefault(x => x.UserSendId == authId && x.UserReceiveId == userId);
            var recevieUserChat = _context.UserChats.FirstOrDefault(x => x.UserSendId == userId && x.UserReceiveId == authId);

            if (authUserChat!.LastSeen == null)
            {
                return _context.Chats.Where(x => x.UserChatId == recevieUserChat!.Id).Count();
            }

            return _context.Chats.Where(x => x.UserChatId == recevieUserChat!.Id && x.DateTime > authUserChat.LastSeen).Count();
        }

        public int GetUnreadChatsCount(long authId)
        {
            return _context.UserChats.ToList().Where(x => x.UserSendId == authId).Select(x => new
            {
                count = new Func<int>(() =>
                {
                    var userchat = _context.UserChats.First(y => y.UserSendId == x.UserReceiveId && y.UserReceiveId == x.UserSendId);

                    if (x.LastSeen == null)
                    {
                        return _context.Chats.Where(y => y.UserChatId == userchat.Id).Count();
                    }

                    return _context.Chats.Where(y => y.UserChatId == userchat.Id && y.DateTime > x.LastSeen).Count();
                })(),
            }).Sum(x => x.count);
        }

        public async Task StoreChat(string sendUser, string receiveUser, string message)
        {
            try
            {
                var userChat = _context.UserChats.FirstOrDefault(x => x.UserSend.Username == sendUser && x.UserReceive.Username == receiveUser);

                if (userChat == null)
                {
                    userChat = new UserChat()
                    {
                        UserReceiveId = _context.Users.First(x => x.Username == receiveUser).Id,
                        UserSendId = _context.Users.First(x => x.Username == sendUser).Id,
                    };

                    _context.Add(userChat);

                    var userChat1 = new UserChat()
                    {
                        UserSendId = _context.Users.First(x => x.Username == receiveUser).Id,
                        UserReceiveId = _context.Users.First(x => x.Username == sendUser).Id,
                    };

                    _context.Add(userChat1);

                    await _context.SaveChangesAsync();
                }

                var chat = new Chat()
                {
                    DateTime = DateTime.Now,
                    Message = message,
                    UserChatId = userChat.Id
                };

                _context.Add(chat);
                await _context.SaveChangesAsync();
            }
            catch (Exception err)
            {
                await Console.Out.WriteLineAsync(err.Message);
            }
        }

        public async Task UpdateLastSeen(long authId, long userId)
        {
            var chatUser = _context.UserChats.FirstOrDefault(x => x.UserSendId == authId && x.UserReceiveId == userId);

            if (chatUser == null)
                return;

            chatUser.LastSeen = DateTime.Now;
            _context.Update(chatUser);
            await _context.SaveChangesAsync();
        }
    }
}
