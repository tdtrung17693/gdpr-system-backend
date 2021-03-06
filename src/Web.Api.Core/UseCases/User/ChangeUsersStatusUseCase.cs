﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.UseCaseRequests.User;
using Web.Api.Core.Dto.UseCaseResponses.User;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.Services;
using Web.Api.Core.Interfaces.UseCases.User;
using DomainEntities = Web.Api.Core.Domain.Entities;

namespace Web.Api.Core.UseCases.User
{
  public sealed class ChangeUsersStatusUseCase : IChangeUsersStatusUseCase
  {
    private IUserRepository _userRepository;
    private DomainEntities.User _currentUser;
    public ChangeUsersStatusUseCase(IUserRepository userRepository, IAuthService authService)
    {
      _userRepository = userRepository;
      _currentUser = authService.GetCurrentUser();
    }

    public async Task<bool> Handle(ChangeUsersStatusRequest message, IOutputPort<ChangeUsersStatusResponse> outputPort)
    {
      var status = message.Status;
      var ids = message.Ids.Where(id => id != _currentUser.Id).ToList();


      var response = await _userRepository.ChangeStatus(ids, status);
      if (response.Success)
      {
        outputPort.Handle(new ChangeUsersStatusResponse());
      } else
      {
        outputPort.Handle(new ChangeUsersStatusResponse(false, response.Errors.First().Description));
      }
      return true;
    }
  }
}
