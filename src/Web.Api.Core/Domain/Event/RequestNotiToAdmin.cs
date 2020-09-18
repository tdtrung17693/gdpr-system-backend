using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Interfaces.Services.Event;

namespace Web.Api.Core.Domain.Event
{
    public class RequestNotiToAdmin : IEvent
    {
        public string RequesterFullName { get; set; }
        public Guid RequestId { get; set; }
        public Guid ServerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ServerName { get; set; }

        public RequestNotiToAdmin(string requesterFullName, string serverName, Guid requestId, Guid serverId, DateTime createdAt)
        {
            RequesterFullName = requesterFullName;
            CreatedAt = createdAt;
            ServerName = serverName;
            RequestId = requestId;
            ServerId = serverId;
        }

    }
}
