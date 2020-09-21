using System;

namespace Web.Api.Core.Dto.Requests
{
  public class RequestAcceptRejectNotificationData
  {
    public Guid RequestId { get; set; }
    public string RequestTitle { get; set; }
    public string NewStatus { get; set; }
  }
}
