using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseResponses.ServerUseCaseResponse
{
    public class BulkServerResponse : UseCaseResponseMessage
    {
        public Guid Id { get; }
        public IEnumerable<string> Errors { get; }

        public BulkServerResponse(IEnumerable<string> errors, bool success = false, string message = null) : base(success, message)
        {
            Errors = errors;
        }

        public BulkServerResponse(Guid id, bool success = false, string message = null) : base(success, message)
        {
            Id = id;
        }
    }
}
