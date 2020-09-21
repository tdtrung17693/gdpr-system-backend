using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseResponses
{ 
    public class BulkRequestResponse : UseCaseResponseMessage
    {
        public IEnumerable<Object> ResponsedRequest { get; }
        public IEnumerable<string> Errors { get; }

        public BulkRequestResponse(IEnumerable<string> errors, bool success = false, string message = null)
            : base(success, message)
        {
            Errors = errors;
        }

        public BulkRequestResponse(IEnumerable<Object> responsedRequest, bool success = true, string message = null)
            : base(success, message)
        {
            ResponsedRequest = responsedRequest;
        }
    }
}
