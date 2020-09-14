using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
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
            await _hubContext.Clients.Group($"conversation:{ev.RequestId}").SendAsync("commentCreated", new
            {
                Author=new
                {
                    FirstName=ev.AuthorFirstName,
                    LastName=ev.AuthorLastName
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