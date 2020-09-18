﻿using FluentEmail.Core;
using FluentEmail.Razor;
using FluentEmail.Smtp;
using Microsoft.Extensions.Configuration;
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
    protected readonly IConfiguration _configuration;
    public MailService(IFluentEmail fluentEmail, IConfiguration configuration)
    {
      _fluentEmail = fluentEmail;
      _configuration = configuration;
    }
    public async Task<bool> SendInvitedNotification(string emailAddress, string username, string rawPassword, string firstName, string lastName)
    {
      var senderName = _configuration["Mail:Name"];
      var email = _fluentEmail
          .To(emailAddress)
          .Subject("GDPR System - Invite Mail")
          .UsingTemplateFromEmbedded(
            "Web.Api.Infrastructure.EmailTemplate.InviteEmail.cshtml",
            new {Email=emailAddress, Username=username, FirstName=firstName, LastName=lastName, RawPassword=rawPassword, SenderName=senderName},
            this.GetType().GetTypeInfo().Assembly);
      
      var response = await email.SendAsync();
      return response.Successful;
    }

    public async Task<bool> SendCreatedRequestToAdmin(List<User> adminList ,string requesterFullname, string servername, Guid requestId, Guid serverId, DateTime createdAt)
    {
            foreach (User admin in adminList)
            {
                var emailAddress = admin.Email;
                var senderName = _configuration["Mail:Name"];
                var email = _fluentEmail
                .To(emailAddress)
                .Subject("GDPR System -New Request")
                .UsingTemplateFromEmbedded(
                    "Web.Api.Infrastructure.EmailTemplate.SendRequestToAdmin.cshtml",
                    new { Requester = requesterFullname, ServerName = servername, RequestId = requestId, ServerId = serverId, CreateAt = createdAt.ToLocalTime().ToString("dd/MM/yyyy HH:mm"), SenderName = senderName},
                    this.GetType().GetTypeInfo().Assembly);

            var response = await email.SendAsync();
        }
        return true;
    }
  }
}
