using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseResponses
{
    public class ExportResponse : UseCaseResponseMessage
    {
        public IEnumerable<Object> ResponsedRequest { get; }
        public IEnumerable<string> Errors { get; }

        public ExportResponse(IEnumerable<string> errors, bool success = false, string message = null)
            : base(success, message)
        {
            Errors = errors;
        }

        public ExportResponse(IEnumerable<Object> responsedRequest, bool success = true, string message = null)
            : base(success, message)
        {
            ResponsedRequest = responsedRequest;
        }
    }
}
