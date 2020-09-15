using System;

namespace Web.Api.Core.Dto.Requests
{
    public class NewRequestNotificationData
    {
        public string Username { get; }
        public string ServerName { get; }
        public Guid ServerId { get; }
        public Guid RequestId { get; }

        public NewRequestNotificationData(string requesterUsername, string serverName, Guid serverId, Guid requestId)
        {
            Username = requesterUsername;
            ServerId = serverId;
            ServerName = serverName;
            RequestId = requestId;
        }
    }
}