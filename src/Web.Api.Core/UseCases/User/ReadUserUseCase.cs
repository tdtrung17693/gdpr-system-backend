using System.Collections.Generic;
using System.Linq;
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
      if (message.UserId != "")
      {
        DomainEntities.User user = await _userRepository.FindById(message.UserId);
        if (user == null)
        {
          var error = new List<string>();
          error.Add("User not found");

          outputPort.Handle(new ReadUserResponse(error));
        }
        else
        {
          outputPort.Handle(new ReadUserResponse(user));
        }
      }
      else
      {
        IPagedCollection<DomainEntities.User> users = _userRepository.FindAll();
        users.FilterBy(message.FilterString);
        users.SortBy(message.SortedBy, message.SortOrder);
        var items = await users.GetItemsForPage(message.Page);
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
