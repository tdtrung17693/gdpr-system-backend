using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Interfaces.Services.Event;

namespace Web.Api.Core.Domain.Event
{
  public class RequestCreated : IEvent
  {
    /*
     *                r.Id as RequestId, r.CreatedAt,
               concat(U.FirstName, U.LastName) as RequesterFullName,
               A.Username as RequesterUsername, A.UserId as RequesterId,
               S.Name as ServerName, S.Id as ServerId

     */
    public string RequesterFullName { get; set; }
    public string RequesterUsername { get; set; }
    public Guid RequesterId { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid ServerId { get; set; }
    public string ServerName { get; set; }
    public Guid RequestId { get; set; }
  }
}
