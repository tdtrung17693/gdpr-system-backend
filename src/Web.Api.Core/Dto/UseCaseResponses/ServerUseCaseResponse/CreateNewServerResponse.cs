using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseResponses
{
    public class CreateNewServerResponse : UseCaseResponseMessage
    {
        public Guid Id { get; }
        public IEnumerable<string> Errors { get; }

        public CreateNewServerResponse(IEnumerable<string> errors, bool success = false, string message = null) : base(success, message)
        {
            Errors = errors;
        }

        public CreateNewServerResponse(Guid id, bool success = false, string message = null) : base(success, message)
        {
            Id = id;
        }
    }
}
