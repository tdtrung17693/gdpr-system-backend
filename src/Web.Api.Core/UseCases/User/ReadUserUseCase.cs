using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.UseCaseRequests.User;
using Web.Api.Core.Dto.UseCaseResponses.User;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases.User;
using DomainEntities = Web.Api.Core.Domain.Entities;

namespace Web.Api.Core.UseCases.User
{
  public sealed class ReadUserUseCase : IReadUserUseCase
  {
    private IUserRepository _userRepository;
    public ReadUserUseCase(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    public async Task<bool> Handle(ReadUserRequest message, IOutputPort<ReadUserResponse> outputPort)
    {
      if (message.UserId.ToString() != Guid.Empty.ToString())
      {
        DomainEntities.User user = await _userRepository.FindById(message.UserId);
        if (user == null)
        {
          outputPort.Handle(new ReadUserResponse(new[] { new Error(Error.Codes.ENTITY_NOT_FOUND, Error.Messages.ENTITY_NOT_FOUND) } ));
        }
        else
        {
          outputPort.Handle(new ReadUserResponse(user));
        }
      }
      else
      {
        IPagedCollection<DomainEntities.User> users = _userRepository.FindAll();
        var filterStr = message.FilterString;
        users.FilterBy(u => u.FirstName.Contains(filterStr) || u.LastName.Contains(filterStr) || u.Email.Contains(filterStr) || u.Account.Username.Contains(filterStr));
        if (message.SortedBy == "Username")
        {
          users.SortBy(u => u.Account.Username, message.SortOrder);
        } else {
          users.SortBy(message.SortedBy, message.SortOrder);
        }
        var items = await users.GetItemsForPage(message.Page, message.PageSize);
        var pagination = new Pagination<DomainEntities.User>
        {
          Items = items,
          TotalItems = users.TotalItems(),
          TotalPages = users.TotalPages(),
          Page = message.Page
        };

        outputPort.Handle(new ReadUserResponse(pagination));
      }

      return true;
    }
  }
}
