using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Event;
using Web.Api.Core.Interfaces.Services.Event;
using Web.Api.Hubs;

namespace Web.Api.EventHandlers
{
    public class BroadcastDeletedComment: IEventHandler<CommentDeleted>
    {
        private readonly IHubContext<ConversationHub> _hubContext;

        public BroadcastDeletedComment(IHubContext<ConversationHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task HandleAsync(CommentDeleted ev)
        {
            await _hubContext.Clients.Group($"conversation:{ev.Id.ToString().ToLower()}").SendAsync("commentDeleted", new
            {
                ev.Id
            }) ;
        }
    }
}
