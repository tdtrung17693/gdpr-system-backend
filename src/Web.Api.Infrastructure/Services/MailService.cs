using FluentEmail.Core;
using FluentEmail.Razor;
using FluentEmail.Smtp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Interfaces.Services;

namespace Web.Api.Infrastructure.Services
{
  public class MailService : IMailService
  {
    protected readonly IFluentEmail _fluentEmail;
    public MailService(IFluentEmail fluentEmail)
    {
        _fluentEmail = fluentEmail;
    }
    public async Task<bool> SendInvitedNotification(string emailAddress, string username, string rawPassword, string firstName, string lastName)
    {
      var email = _fluentEmail
          .To(emailAddress)
          .Subject("GDPR System - Invite Mail")
          .UsingTemplateFromEmbedded(
            "Web.Api.Infrastructure.Pages.InviteEmail.cshtml",
            new {Email=emailAddress, Username=username, FirstName=firstName, LastName=lastName, RawPassword=rawPassword},
            this.GetType().GetTypeInfo().Assembly);
      
      var response = await email.SendAsync();
      return response.Successful;
    }
  }
}
