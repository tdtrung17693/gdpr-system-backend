using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Domain.Entities;

namespace Web.Api.Core.Dto.GatewayResponses.Repositories
{
    public sealed class ManageServerCustomerResponse : BaseGatewayResponse
    {
        public ManageServerCustomerResponse(bool success = false, IEnumerable<Error> errors = null) : base(success, errors)
        {
        }
    }
}
