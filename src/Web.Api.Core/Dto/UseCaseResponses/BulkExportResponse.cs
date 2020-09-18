using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseResponses
{
    public class BulkExportResponse : UseCaseResponseMessage
    {
        public IEnumerable<Object> ResponsedRequest { get; }
        public IEnumerable<Error> Errors { get; }
        public BulkExportResponse(bool success = false, string message = null) : base(success, message)
        {
        }

        public BulkExportResponse(IEnumerable<Error> errors, bool success = false, string message = null) : base(success, message)
        {
            Errors = errors;
        }
        public BulkExportResponse(IEnumerable<Object> responsedRequest, bool success = true, string message = null)
            : base(success, message)
        {
            ResponsedRequest = responsedRequest;
        }
    }
}
