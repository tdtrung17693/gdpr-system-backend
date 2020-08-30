using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Core.Dto.GatewayResponses.Repositories
{
    public sealed class CreateServerResponse : BaseGatewayResponse
    {
        public Guid Id { get; }
        public CreateServerResponse(Guid id, bool success = false, IEnumerable<Error> errors = null) : base(success, errors)
        {
            Id = id;
        }
    }
}
