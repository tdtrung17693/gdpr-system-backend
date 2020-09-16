using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class GetEachRequestRequest : IUseCaseRequest<GetEachRequestResponse>
    {
        public GetEachRequestRequest(string requestId)
        {
            RequestId = requestId;
        }

        public string RequestId { get; set; }

    }
}