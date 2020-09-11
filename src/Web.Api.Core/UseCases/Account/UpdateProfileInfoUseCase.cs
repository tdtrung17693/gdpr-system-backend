using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Dto.GatewayResponses.Repositories;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseRequests.Account;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Dto.UseCaseResponses.Account;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases;
using Web.Api.Core.Interfaces.UseCases.Account;

namespace Web.Api.Core.UseCases.Account
{
  public sealed class UpdateProfileInfoUseCase : IUpdateProfileInfoUseCase
  {
    protected IUserRepository _userRepository;
    public UpdateProfileInfoUseCase(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }
    public async Task<bool> Handle(UpdateProfileInfoRequest message, IOutputPort<UpdateProfileInfoResponse> outputPort)
    {
      var id = message.UserId;
      var firstName = message.FirstName;
      var lastName = message.LastName;

      var response = await _userRepository.Update(id, firstName, lastName);

      if (!response.Success)
      {
        outputPort.Handle(new UpdateProfileInfoResponse(response.Errors));
      }

      outputPort.Handle(new UpdateProfileInfoResponse(response.UpdatedFields));
      return true;
    }
  }
}
