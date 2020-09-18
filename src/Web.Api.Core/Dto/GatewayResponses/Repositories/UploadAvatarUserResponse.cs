using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Domain.Entities;

namespace Web.Api.Core.Dto.GatewayResponses.Repositories
{
    public sealed class UploadAvatarUserResponse : BaseGatewayResponse
    {
        public Guid Id { get; set; }
        public  UploadAvatarUserResponse(Guid id, bool success = false, IEnumerable<Error> errors = null) : base(success, errors)
        {
            Id = id;
        }

        public UploadAvatarUserResponse(bool success = false, IEnumerable<Error> errors = null) : base(success, errors)
        {
        }
    }
}
