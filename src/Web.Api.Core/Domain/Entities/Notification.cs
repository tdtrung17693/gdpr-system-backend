using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Core.Domain.Entities
{
  public class Notification : BaseEntity
  {
    public Notification(Guid? id, Guid? fromUserId, Guid toUserId, string notificationType, string data, Guid? createdBy = null, DateTime? createdAt = null)
       : base(id, createdAt, createdBy)
    {
      FromUserId = fromUserId;
      ToUserId = toUserId;
      NotificationType = notificationType;
      Data = data;
      IsRead = false;
    }


    public Guid? FromUserId { get; set; }
    public virtual User? FromUser { get; set; }
    public Guid ToUserId { get; set; }
    public virtual User ToUser { get; set; }

    // JSON serialized string of notification data
    public string Data { get; set; }
    public string NotificationType { get; set; }
    public bool IsRead { get; set; }
  }
}
