using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Dto.UseCaseResponses.User;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests.User
{
  public class ChangeUsersStatusRequest : IUseCaseRequest<ChangeUsersStatusResponse>
  {
    public ChangeUsersStatusRequest(Guid[] ids, bool status)
    {
      Ids = ids;
      Status = status;
    }

    public Guid[] Ids { get; set; }
    public bool Status { get; set; }
  }
}
