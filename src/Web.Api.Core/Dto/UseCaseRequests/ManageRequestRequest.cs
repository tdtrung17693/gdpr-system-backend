using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class ManageRequestRequest : IUseCaseRequest<ManageRequestResponse>
    {
        public ManageRequestRequest(string userId, string answer, string status, string requestId)
        {
            UserId = userId;
            Answer = answer;
            Status = status;
            RequestId = requestId;
        }

        public string UserId { get; set; }
        public string Answer { get; set; }
        public string Status { get; set; }
        public string RequestId { get; set; }
    }
}
