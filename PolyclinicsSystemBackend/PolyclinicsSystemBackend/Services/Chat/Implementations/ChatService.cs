// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.SignalR;
// using Microsoft.Extensions.Logging;
// using PolyclinicsSystemBackend.Data;
// using PolyclinicsSystemBackend.Dtos.Chat;
// using PolyclinicsSystemBackend.Dtos.Generics;
// using PolyclinicsSystemBackend.Services.Chat.Interface;
// using PolyclinicsSystemBackend.SignalR;
//
// namespace PolyclinicsSystemBackend.Services.Chat.Implementations
// {
//     public class ChatService : IChatService
//     {
//         private readonly ILogger<ChatService> _logger;
//         private readonly AppDbContext _appDbContext;
//         private readonly IHubContext<ChatHub> _hubContext;
//
//         public ChatService(ILogger<ChatService> logger,
//             AppDbContext appDbContext,
//             IHubContext<ChatHub> hubContext)
//         {
//             _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//             _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
//             _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
//         }
//         
//         public Task<GenericResponse<string, List<MessageDto>>> LoadMessages(int appointmentId)
//         {
//             throw new System.NotImplementedException();
//         }
//
//         public Task<GenericResponse<string, string>> GetAppointmentGroup(int appointmentId)
//         {
//             throw new System.NotImplementedException();
//         }
//
//         public Task<GenericResponse<string, bool>> RemoveAppointmentGroup(int appointmentId)
//         {
//             throw new System.NotImplementedException();
//         }
//
//         public Task<GenericResponse<string, bool>> ConnectUserToAppointmentGroup(int appointmentId, string userId)
//         {
//             throw new System.NotImplementedException();
//         }
//
//         public Task<GenericResponse<string, bool>> DisconnectUserFromAppointmentGroup(int appointmentId, string userId)
//         {
//             throw new NotImplementedException();
//         }
//     }
// }