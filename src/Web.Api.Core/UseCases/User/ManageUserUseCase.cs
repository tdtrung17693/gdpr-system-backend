using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Dto.UseCaseRequests.User;
using Web.Api.Core.Dto.UseCaseResponses.User;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.UseCases.User;
using DomainEntities = Web.Api.Core.Domain.Entities;

namespace Web.Api.Core.UseCases.User
{
  public class ManageUserUseCase
  {
    private ICreateUserUseCase _createUserUseCase;
    private IReadUserUseCase _readUserUseCase;
    private IUpdateUserUseCase _updateUserUseCase;
    private IDeleteUserUseCase _deleteUserUseCase;
    public ManageUserUseCase(
      ICreateUserUseCase createUserUseCase,
      IReadUserUseCase readUserUseCase,
      IUpdateUserUseCase updateUserUseCase,
      IDeleteUserUseCase deleteUserUseCase
      )
    {
      _createUserUseCase = createUserUseCase;
      _readUserUseCase = readUserUseCase;
      _updateUserUseCase = updateUserUseCase;
      _deleteUserUseCase = deleteUserUseCase;
    }

    public async Task<bool> Create(CreateUserRequest message, IOutputPort<CreateUserResponse> outputPort)
    {
      return await _createUserUseCase.Handle(message, outputPort);
    }

    public async Task<bool> Update(UpdateUserRequest message, IOutputPort<UpdateUserResponse> outputPort)
    {
      return await _updateUserUseCase.Handle(message, outputPort);
    }

    public async Task<bool> Delete(DeleteUserRequest message, IOutputPort<DeleteUserResponse> outputPort)
    {
      return await _deleteUserUseCase.Handle(message, outputPort);
    }

    public async Task<bool> Read(ReadUserRequest message, IOutputPort<ReadUserResponse> outputPort)
    {
      await _readUserUseCase.Handle(message, outputPort);
      return true;
    }
  }
}
