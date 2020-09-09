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

            builder.Register(c => {
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
                return new Email(c.Resolve<ITemplateRenderer>(), c.Resolve<ISender>(), "test@mail.com", "Test Admin");
            }).As<IFluentEmail>().InstancePerDependency();
            builder.Register(c => new RazorRenderer()).As<ITemplateRenderer>().SingleInstance();
            builder.Register(c => new SmtpSender(() => {
                var client = new SmtpClient("smtp.mailtrap.io", 587);
                client.Credentials = new System.Net.NetworkCredential("4c688794666254", "d6761786a3e9b8");
                client.EnableSsl = true;
                return client;
            })).As<ISender>().InstancePerLifetimeScope();
        }
    }
}
