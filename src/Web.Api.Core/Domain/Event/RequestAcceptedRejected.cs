using System;
using Web.Api.Core.Interfaces.Services.Event;

namespace Web.Api.Core.Domain.Event
{
    public class RequestAcceptedRejected : IEvent
    {
        public Guid RequestId { get; set; }
        public Guid UpdatedBy { get; set; }
        public string NewStatus { get; set; }
    }
}