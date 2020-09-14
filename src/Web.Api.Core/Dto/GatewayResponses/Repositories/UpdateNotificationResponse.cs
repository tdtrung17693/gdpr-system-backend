using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Core.Dto.GatewayResponses.Repositories
{
  public class UpdateNotificationResponse : BaseGatewayResponse
  {
    public UpdateNotificationResponse(IEnumerable<Error> errors) : base(false, errors)
    {
    }
    public UpdateNotificationResponse() : base(true,
        null)
    {
    }
  }
}
