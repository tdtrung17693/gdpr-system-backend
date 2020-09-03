using Autofac;
using Web.Api.Core.Interfaces.UseCases;
using Web.Api.Core.Interfaces.UseCases.User;
using Web.Api.Core.UseCases;
using Web.Api.Core.UseCases.User;

namespace Web.Api.Core
{
  public class CoreModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      builder.RegisterType<ReadUserUseCase>().As<IReadUserUseCase>().InstancePerLifetimeScope();
      builder.RegisterType<CreateUserUseCase>().As<ICreateUserUseCase>().InstancePerLifetimeScope();
      builder.RegisterType<DeleteUserUseCase>().As<IDeleteUserUseCase>().InstancePerLifetimeScope();
      builder.RegisterType<UpdateUserUseCase>().As<IUpdateUserUseCase>().InstancePerLifetimeScope();
      builder.RegisterType<LoginUseCase>().As<ILoginUseCase>().InstancePerLifetimeScope();
      builder.RegisterType<ManageUserUseCase>().As<ManageUserUseCase>().InstancePerLifetimeScope();
    }
  }
}
