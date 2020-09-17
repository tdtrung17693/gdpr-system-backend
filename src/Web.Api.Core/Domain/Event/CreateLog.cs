using System;
using Web.Api.Core.Interfaces.Services.Event;

namespace Web.Api.Core.Domain.Event
{
    public class CreateLog : IEvent
    {
        public Guid? RequestId { get; }
        public string UpdatedField { get; }
        public string UpdatedState { get; }
        public string PreviousState { get; }
        public string Message { get; }
        public Guid? CreatedBy { get; }

        public CreateLog(Guid? requestId, string updatedField, string updatedState, string previousState, string message, Guid? createdBy)
        {
            RequestId = requestId;
            UpdatedField = updatedField;
            UpdatedState = updatedState;
            PreviousState = previousState;
            Message = message;
            CreatedBy = createdBy;
        }
    }
}