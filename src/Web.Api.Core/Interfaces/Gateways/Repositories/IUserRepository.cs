using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.GatewayResponses.Repositories;
using Web.Api.Core.Dto.UseCaseRequests;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
  public interface IUserRepository
  {
    Task<CreateUserResponse> Create(User userInfo, string userName, string password, Guid creator);
    Task<User> FindById(Guid id);
    Task<User> FindByName(string userName);
    Task<User> FindByEmail(string email);
    Task<bool> CheckPassword(User user, string password);
    IPagedCollection<User> FindAll();
    Task<UpdateUserResponse> UpdateProfileInfo(User user, string firstName, string lastName);
    Task<UpdateUserResponse> Update(Guid id, Guid roleId, bool status);
    Task<UpdateUserResponse> ChangeStatus(ICollection<Guid> ids, bool status);
    Task<CreateUserResponse> Delete(User user);
    Task<UpdateUserResponse> ChangePassword(User user, string newPassword);

        //Khoa
    Task<UploadAvatarUserResponse> UploadFirstAvatar(UploadAvatarRequest request);
  }
}
