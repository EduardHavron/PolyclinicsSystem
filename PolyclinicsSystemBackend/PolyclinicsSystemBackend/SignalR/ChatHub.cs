// using System;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.SignalR;
// using Newtonsoft.Json;
// using PolyclinicsSystemBackend.Dtos.Chat;
// using PolyclinicsSystemBackend.Services.Chat.Interface;
//
// namespace PolyclinicsSystemBackend.SignalR
// {
//     [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//     public class ChatHub : Hub
//     {
//         private readonly IChatService _chatService;
//
//         public ChatHub(IChatService chatService)
//         {
//             _chatService = chatService ?? throw new ArgumentNullException(nameof(chatService));
//         }
//         
//         public async Task SendToGroup(MessageDto message, string groupName)
//         {
//             var messageJson = JsonConvert.SerializeObject(message);
//             await Clients.Groups(groupName).SendAsync(messageJson);
//         }
//
//         public async Task ConnectToGroup(string groupName)
//         {
//             await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
//         }
//
//         public async Task DisconnectFromGroup(string groupName)
//         {
//             await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
//         }
//
//         public async Task RemoveGroup(string groupName)
//         {
//             await Groups.
//         }
//     }
// }