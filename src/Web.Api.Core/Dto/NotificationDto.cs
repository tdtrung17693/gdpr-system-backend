using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Dto.UseCaseResponses.User;

namespace Web.Api.Core.Dto
{
  public class NotificationDto
  {
    public Guid Id { get; set; }
    public string Data { get; set; }
    public string NotificationData { get; set; }
    public string NotificationType { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? FromUserId { get; set; }
    public string? FromUserName { get; set; }
    public Guid ToUserId { get; set; }
    public string ToUserName { get; set; }
  }
}
