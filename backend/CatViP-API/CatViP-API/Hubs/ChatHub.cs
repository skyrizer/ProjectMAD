using CatViP_API.Models;
using CatViP_API.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace CatViP_API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task SendPrivateMessage(string sendUser, string receiveUser, string message)
        {
            await Clients.All.SendAsync($"ReceiveMessageFrom{sendUser}To{receiveUser}", message);
            await _chatService.PushNotification(sendUser, receiveUser, message);
            await _chatService.StoreChat(sendUser, receiveUser, message);
        }
    }
}
