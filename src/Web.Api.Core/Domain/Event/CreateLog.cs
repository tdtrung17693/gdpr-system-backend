using System;
using Web.Api.Core.Interfaces.Services.Event;

namespace Web.Api.Core.Domain.Event
{
  public class CreateLog : IEvent
  {
    public Guid? RequestId { get; set; }
    public string UpdatedField { get; set; }
    public string UpdatedState { get; set; }
    public string PreviousState { get; set; }
    public string Message { get; set; }
    public Guid? CreatedBy { get; set; }

    public CreateLog(Guid? requestId, string updatedField, string updatedState, string previousState, string message,
      Guid? createdBy)
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
