using System;
using System.Collections.Generic;
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
  public sealed class ReadUserUseCase : IReadUserUseCase
  {
    private IUserRepository _userRepository;
    private Core.Domain.Entities.User _currentUser;
    public ReadUserUseCase(IUserRepository userRepository, IAuthService authService)
    {
      _userRepository = userRepository;
      // Inject authservice here to get the current logged in user, so that 
      // their id could be accessed. 
      // The Id is used in the query to exclude f
      _currentUser = authService.GetCurrentUser();
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
        if (filterStr.Contains(",") && filterStr.Contains(":") || filterStr.Contains(":"))
        {
          var filterCriteria = filterStr.Split(",");
          foreach (var crit in filterCriteria)
          {
            var keyVal = crit.Split(":");

            users.FilterBy(keyVal[0], keyVal[1]);
          }
        } else
        {
          users.FilterBy(u => u.FirstName.Contains(filterStr) || u.LastName.Contains(filterStr) || u.Email.Contains(filterStr) || u.Account.Username.Contains(filterStr));
        }
        users.FilterBy(u => u.Id != _currentUser.Id);
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
