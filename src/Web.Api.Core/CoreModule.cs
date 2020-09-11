using Autofac;
using Web.Api.Core.Interfaces.UseCases;
using Web.Api.Core.Interfaces.UseCases.ServerInterface;
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
      builder.RegisterType<ChangeUsersStatusUseCase>().As<IChangeUsersStatusUseCase>().InstancePerLifetimeScope();
      builder.RegisterType<ExportCustomerUseCase>().As<IExportCustomerUseCase>().InstancePerLifetimeScope();  
      builder.RegisterType<CRUDCustomerUseCase>().As<ICRUDCustomerUseCase>().InstancePerLifetimeScope();  
      builder.RegisterType<LoginUseCase>().As<ILoginUseCase>().InstancePerLifetimeScope();
      builder.RegisterType<ManageUserUseCase>().As<ManageUserUseCase>().InstancePerLifetimeScope();
      builder.RegisterType<CreateServerUseCase>().As<ICreateServerUseCase>().InstancePerLifetimeScope();
      builder.RegisterType<UpdateServerUseCase>().As<IUpdateServerUseCase>().InstancePerLifetimeScope();
      builder.RegisterType<BulkServerUseCase>().As<IBulkServerUseCase>().InstancePerLifetimeScope();
    }
  }
}
