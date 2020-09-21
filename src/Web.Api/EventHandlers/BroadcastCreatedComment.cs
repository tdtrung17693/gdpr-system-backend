using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SlackAPI;
using Web.Api.Core.Domain.Event;
using Web.Api.Core.Interfaces.Services.Event;
using Web.Api.Hubs;

namespace Web.Api.EventHandlers
{
    public class BroadcastCreatedComment : IEventHandler<CommentCreated>
    {
        private readonly IHubContext<ConversationHub> _hubContext;

        public BroadcastCreatedComment(IHubContext<ConversationHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task HandleAsync(CommentCreated ev)
        {
            byte[] file = System.IO.File.ReadAllBytes(ev.AuthorAvatar);
            var content = Convert.ToBase64String(file);
            await _hubContext.Clients.Group($"conversation:{ev.RequestId.ToString().ToLower()}").SendAsync("commentCreated", new
            {
                Author=new
                {
                    FirstName=ev.AuthorFirstName,
                    LastName=ev.AuthorLastName,
                    Avatar=content
                },
                ev.CreatedAt,
                ev.Content,
                ev.Id,
                ev.ParentId,
                ev.RequestId
            });
        }
    }
}
