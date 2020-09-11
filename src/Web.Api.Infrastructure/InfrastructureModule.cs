using Autofac;
using Web.Api.Infrastructure.Auth;
using Web.Api.Core.Interfaces.Services;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Infrastructure.Data.EntityFramework.Repositories;
using FluentEmail.Core;
using FluentEmail.Core.Interfaces;
using FluentEmail.Razor;
using FluentEmail.Smtp;
using System.Net.Mail;
using Web.Api.Infrastructure.Event;
using Web.Api.Core.Interfaces.Services.Event;
using Microsoft.AspNetCore.Http;
using Web.Api.Core.Domain.Event;
using Web.Api.Domain.Event;
using Web.Api.Infrastructure.Services;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace Web.Api.Infrastructure
{
  public class InfrastructureModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {

      builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
      builder.RegisterType<PermissionRepository>().As<IPermissionRepository>().InstancePerLifetimeScope();
      builder.RegisterType<RoleRepository>().As<IRoleRepository>().InstancePerLifetimeScope();

      // Services
      builder.RegisterType<JwtFactory>().As<IJwtFactory>().SingleInstance();
      builder.RegisterType<AuthService>().As<IAuthService>().InstancePerLifetimeScope();
      builder.RegisterType<MailService>().As<IMailService>().InstancePerLifetimeScope();

      builder.Register(c =>
      {
        var eventBus = new EventBus(c.Resolve<IHttpContextAccessor>());
        eventBus.AddEventHandler<UserCreated, SendInviteMail>();
        return eventBus;
      }).As<IDomainEventBus>().SingleInstance();

      builder.Register(c =>
      {
        var handler = new SendInviteMail(c.Resolve<IMailService>());
        return handler;
      }).As<SendInviteMail>().SingleInstance();

      // FluentEmail
      builder.Register(c =>
      {
        var config = c.Resolve<IConfiguration>();
        var adminEmail = config["Mail:From"];
        var adminName = config["Mail:AdminName"];

        return new Email(c.Resolve<ITemplateRenderer>(), c.Resolve<ISender>(), adminEmail, adminName);
      }).As<IFluentEmail>().InstancePerDependency();
      builder.Register(c => new RazorRenderer()).As<ITemplateRenderer>().SingleInstance();
      builder.Register(c =>
      {
        var config = c.Resolve<IConfiguration>();
        var smtpServer = config["Mail:Host"];
        int smtpPort = int.TryParse(config["Mail:Port"], out smtpPort) ? smtpPort : 587;
        var smtpUsername = config["Mail:Username"];
        var smtpPassword = config["Mail:Password"];

        return new SmtpSender(() =>
        {
          var client = new SmtpClient(smtpServer, smtpPort);
          client.Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword);
          client.EnableSsl = true;
          return client;
        });
      }).As<ISender>().InstancePerLifetimeScope();
    }
  }
}
