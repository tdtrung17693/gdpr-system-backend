using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseResponses
{
    public class GetEachRequestResponse : UseCaseResponseMessage
    {
        public RequestDetail RequestDetails { get; }
        public IEnumerable<Error> Errors { get; }
        public GetEachRequestResponse(IEnumerable<Error> errors, bool success = false, string message = null) : base(success, message)
        {
            Errors = errors;
        }

        public GetEachRequestResponse(RequestDetail requestDetails, bool success = false, string message = null) : base(success, message)
        {
            RequestDetails = requestDetails;
        }
    }
}
