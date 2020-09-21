using System;
using Web.Api.Core.Dto.UseCaseResponses.User;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests.User
{
  public class UpdateUserRequest : IUseCaseRequest<UpdateUserResponse>
  {
    public UpdateUserRequest(Guid id, Guid roleId, bool status)
    {
      Id = id;
      RoleId = roleId;
      Status = status;
    }
    public Guid Id { get; set; }
    public Guid RoleId { get; set; }
    public bool Status { get; set; }
  }
}
