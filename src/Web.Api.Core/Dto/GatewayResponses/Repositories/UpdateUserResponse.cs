using System.Collections.Generic;

namespace Web.Api.Core.Dto.GatewayResponses.Repositories
{
  public sealed class UpdateUserResponse : BaseGatewayResponse
  {
    public UpdateUserResponse(bool success, IEnumerable<Error> errors = null) : base(success, errors)
    {
    }
  }
}
