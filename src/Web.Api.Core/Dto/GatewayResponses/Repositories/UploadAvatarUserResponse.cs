using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Domain.Entities;

namespace Web.Api.Core.Dto.GatewayResponses.Repositories
{
    public sealed class UploadAvatarUserResponse : BaseGatewayResponse
    {
        public  UploadAvatarUserResponse(bool success = false, IEnumerable<Error> errors = null) : base(success, errors)
        {
        }
    }
}
