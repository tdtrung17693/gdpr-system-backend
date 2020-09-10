using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseResponses
{
    public class ExportCustomerResponse: UseCaseResponseMessage
    {
        public IEnumerable<Object> ResponsedRequest { get; }
        public IEnumerable<string> Errors { get; }

        public ExportCustomerResponse(IEnumerable<string> errors, bool success = false, string message = null) 
            : base(success, message)
        {
            Errors = errors;
        }

        public ExportCustomerResponse(IEnumerable<Object> responsedRequest, bool success = false, string message = null) 
            : base(success, message)
        {
            ResponsedRequest = responsedRequest;
        }
    }
}
