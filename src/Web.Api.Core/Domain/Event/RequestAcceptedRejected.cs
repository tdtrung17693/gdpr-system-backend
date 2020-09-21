using System;
using Web.Api.Core.Interfaces.Services.Event;

namespace Web.Api.Core.Domain.Event
{
  public class RequestAcceptedRejected : IEvent
  {
    public Guid RequestId { get; set; }
    public Guid UpdatedBy { get; set; }
    public string OldStatus { get; set; }
    public string NewStatus { get; set; }
    public string ApproverFullName { get; set; }
    public string RequesterFullName { get; set; }
    public string RequesterEmail { get; set; }
    public string RequestTitle { get; set; }
    public Guid RequesterId { get; set; }
    public string RequesterUsername { get; set; }
  }
}
