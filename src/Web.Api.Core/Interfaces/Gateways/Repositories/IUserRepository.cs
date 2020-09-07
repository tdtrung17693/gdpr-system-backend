using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.GatewayResponses.Repositories;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
  public interface IUserRepository
  {
    Task<CreateUserResponse> Create(User userInfo, string userName, string password);
    Task<User> FindById(string id);
    Task<User> FindByName(string userName);
    Task<User> FindByEmail(string email);
    Task<bool> CheckPassword(User user, string password);
    IPagedCollection<User> FindAll();
    Task<CreateUserResponse> Update(User user);
    Task<CreateUserResponse> Delete(User user);
  }
}
