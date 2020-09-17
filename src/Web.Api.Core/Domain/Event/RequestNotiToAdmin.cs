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
        public string RequesterUsername { get; set; }
        public Guid RequesterId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid ServerId { get; set; }
        public string ServerName { get; set; }
        public Guid RequestId { get; set; }

    }
}
