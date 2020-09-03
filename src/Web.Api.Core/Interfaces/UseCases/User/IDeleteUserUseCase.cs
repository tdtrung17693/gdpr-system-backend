using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Dto.UseCaseRequests.User;
using Web.Api.Core.Dto.UseCaseResponses.User;

namespace Web.Api.Core.Interfaces.UseCases.User
{
  public interface IDeleteUserUseCase : IUseCaseRequestHandler<DeleteUserRequest, DeleteUserResponse>
  {
  }
}
