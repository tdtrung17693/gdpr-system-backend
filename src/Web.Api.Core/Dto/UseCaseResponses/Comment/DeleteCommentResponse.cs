using System;
using System.Collections.Generic;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseResponses.Comment
{
    public class DeleteCommentResponse : UseCaseResponseMessage
    {
        public Guid Id { get; }
        public IEnumerable<Error> Errors { get; }

        public DeleteCommentResponse(IEnumerable<Error> errors, bool success = false, string message = null) : base(success, message)
        {
            Errors = errors;
        }

        public DeleteCommentResponse(Guid id, bool success = false, string message = null) : base(success, message)
        {
            Id = id;
        }
    }
}