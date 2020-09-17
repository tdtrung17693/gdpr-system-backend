using System;
using System.Collections.Generic;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseResponses.Comment
{
    public class DeleteCommentResponse : UseCaseResponseMessage
    {
        public Guid Id { get; }
        public IEnumerable<Error> Errors { get; }

        public DeleteCommentResponse(IEnumerable<Error> errors, string message = null) : base(false, message)
        {
            Errors = errors;
        }

        public DeleteCommentResponse(string message = null) : base(true, message)
        {
        }
    }
}