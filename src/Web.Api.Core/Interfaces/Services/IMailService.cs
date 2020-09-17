using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;

namespace Web.Api.Core.Interfaces.Services
{
  public interface IMailService
  {
    Task<bool> SendInvitedNotification(string emailAddress, string username, string rawPassword, string firstName, string lastName);
    // TODO: Add later
    //bool SendAdminNotification(Request request);
  }
}
