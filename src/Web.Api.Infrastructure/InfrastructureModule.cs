using Autofac;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.Services;
using Web.Api.Infrastructure.Auth;
using Web.Api.Infrastructure.Data.EntityFramework.Repositories;
using FluentEmail.Core;
using FluentEmail.Core.Interfaces;
using FluentEmail.Razor;
using FluentEmail.Smtp;
using System.Net.Mail;
using Web.Api.Infrastructure.Services;
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
      builder.RegisterType<ServerRepository>().As<IServerRepository>().InstancePerLifetimeScope();
      builder.RegisterType<CustomerRepository>().As<ICustomerRepository>().InstancePerLifetimeScope();
      builder.RegisterType<RequestRepository>().As<IRequestRepository>().InstancePerLifetimeScope();
      builder.RegisterType<CommentRepository>().As<ICommentRepository>().InstancePerLifetimeScope();
      builder.RegisterType<NotificationRepository>().As<INotificationRepository>().InstancePerLifetimeScope();

      // Services
      builder.RegisterType<JwtFactory>().As<IJwtFactory>().SingleInstance();
      builder.RegisterType<AuthService>().As<IAuthService>().InstancePerLifetimeScope();
      builder.RegisterType<MailService>().As<IMailService>().InstancePerLifetimeScope();


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
