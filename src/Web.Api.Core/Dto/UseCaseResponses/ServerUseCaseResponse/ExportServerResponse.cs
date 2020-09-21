using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseResponses
{
    public class ExportServerResponse : UseCaseResponseMessage
    {
        public IEnumerable<Object> ResponsedRequest { get; }
        public IEnumerable<string> Errors { get; }

        public ExportServerResponse(IEnumerable<string> errors, bool success = false, string message = null)
            : base(success, message)
        {
            Errors = errors;
        }

        public ExportServerResponse(IEnumerable<Object> responsedRequest, bool success = false, string message = null)
            : base(success, message)
        {
            ResponsedRequest = responsedRequest;
        }
    }
}
