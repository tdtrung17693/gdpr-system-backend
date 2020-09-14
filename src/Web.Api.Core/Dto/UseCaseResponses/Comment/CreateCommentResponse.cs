using System;
using System.Collections.Generic;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseResponses.Comment
{
    public class CreateCommentResponse : UseCaseResponseMessage
    {
        public Guid Id { get; }
        public DateTime CreatedAt  { get; }
        public IEnumerable<Error> Errors { get; }

        public CreateCommentResponse(IEnumerable<Error> errors, bool success = false, string message = null) : base(success, message)
        {
            Errors = errors;
        }

        public CreateCommentResponse(Guid id, DateTime createdAt, bool success = false, string message = null) : base(success, message)
        {
            Id = id;
            CreatedAt = createdAt;
        }
    }
}