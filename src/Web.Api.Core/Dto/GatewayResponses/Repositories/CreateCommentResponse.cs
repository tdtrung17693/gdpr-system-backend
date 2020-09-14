using System;
using System.Collections;
using System.Collections.Generic;

namespace Web.Api.Core.Dto.GatewayResponses.Repositories
{
    public class CreateCommentResponse : BaseGatewayResponse
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public CreateCommentResponse(IEnumerable<Error> errors) : base(false, errors)
        {
        }
        public CreateCommentResponse(Guid id, DateTime createdAt) : base(true,
            null)
        {
            Id = id;
            CreatedAt = createdAt;
        }
    }
}