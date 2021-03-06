﻿using Autofac;
using Web.Api.Core.Interfaces.UseCases;
using Web.Api.Core.Interfaces.UseCases.Account;
using Web.Api.Core.Interfaces.UseCases.Comment;
using Web.Api.Core.Interfaces.UseCases.ServerInterface;
using Web.Api.Core.Interfaces.UseCases.User;
using Web.Api.Core.Interfaces.UseCases.ICustomerUseCases;
using Web.Api.Core.Interfaces.UseCases.IRequestUseCases;
using Web.Api.Core.UseCases.CustomerUseCases;
using Web.Api.Core.UseCases;
using Web.Api.Core.UseCases.Account;
using Web.Api.Core.UseCases.Comment;
using Web.Api.Core.UseCases.Request;
using Web.Api.Core.UseCases.User;
using CRUDCustomerUseCase = Web.Api.Core.UseCases.CustomerUseCases.CRUDCustomerUseCase;
using ExportCustomerUseCase = Web.Api.Core.UseCases.CustomerUseCases.ExportCustomerUseCase;
using ManageServerUseCase = Web.Api.Core.UseCases.CustomerUseCases.ManageServerUseCase;

namespace Web.Api.Core
{
  public class CoreModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      builder.RegisterType<ManageServerUseCase>().As<IManageServerUseCase>().InstancePerLifetimeScope();  
      builder.RegisterType<UpdateProfileInfoUseCase>().As<IUpdateProfileInfoUseCase>().InstancePerLifetimeScope();
      builder.RegisterType<ChangePasswordUseCase>().As<IChangePasswordUseCase>().InstancePerLifetimeScope();
      builder.RegisterType<ResetPasswordUseCase>().As<IResetPasswordUseCase>().InstancePerLifetimeScope();
      
      builder.RegisterType<ReadUserUseCase>().As<IReadUserUseCase>().InstancePerLifetimeScope();
      builder.RegisterType<CreateUserUseCase>().As<ICreateUserUseCase>().InstancePerLifetimeScope();
      builder.RegisterType<DeleteUserUseCase>().As<IDeleteUserUseCase>().InstancePerLifetimeScope();
      builder.RegisterType<UpdateUserUseCase>().As<IUpdateUserUseCase>().InstancePerLifetimeScope();
      builder.RegisterType<ChangeUsersStatusUseCase>().As<IChangeUsersStatusUseCase>().InstancePerLifetimeScope();
      builder.RegisterType<ManageUserUseCase>().As<ManageUserUseCase>().InstancePerLifetimeScope();
      
      builder.RegisterType<CreateCommentUseCase>().As<ICreateCommentUseCase>().InstancePerLifetimeScope();
      builder.RegisterType<DeleteCommentUseCase>().As<IDeleteCommentUseCase>().InstancePerLifetimeScope();

      builder.RegisterType<ExportCustomerUseCase>().As<IExportCustomerUseCase>().InstancePerLifetimeScope();  
      builder.RegisterType<CRUDCustomerUseCase>().As<ICRUDCustomerUseCase>().InstancePerLifetimeScope();  

      builder.RegisterType<LoginUseCase>().As<ILoginUseCase>().InstancePerLifetimeScope();

      builder.RegisterType<ReadServerUseCase>().As<IReadServerUseCase>().InstancePerLifetimeScope();
      builder.RegisterType<CreateServerUseCase>().As<ICreateServerUseCase>().InstancePerLifetimeScope();
      builder.RegisterType<UpdateServerUseCase>().As<IUpdateServerUseCase>().InstancePerLifetimeScope();
      builder.RegisterType<BulkServerUseCase>().As<IBulkServerUseCase>().InstancePerLifetimeScope();
      builder.RegisterType<ExportServerUseCase>().As<IExportServerUseCase>().InstancePerLifetimeScope();

      builder.RegisterType<CreateRequestUseCase>().As<ICreateRequestUseCase>().InstancePerLifetimeScope();
      builder.RegisterType<UpdateRequestUseCase>().As<IUpdateRequestUseCase>().InstancePerLifetimeScope();
      builder.RegisterType<BulkExportUseCase>().As<IBulkExportUseCase>().InstancePerLifetimeScope();
    }
  }
}
