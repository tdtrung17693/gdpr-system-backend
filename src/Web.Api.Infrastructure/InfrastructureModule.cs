using Autofac;
using Web.Api.Infrastructure.Auth;
using Web.Api.Core.Interfaces.Services;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Infrastructure.Data.EntityFramework.Repositories;

namespace Web.Api.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PermissionRepository>().As<IPermissionRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RoleRepository>().As<IRoleRepository>().InstancePerLifetimeScope();
            builder.RegisterType<JwtFactory>().As<IJwtFactory>().SingleInstance();
            builder.RegisterType<AuthService>().As<IAuthService>().InstancePerLifetimeScope();
        }
    }
}
