using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class ManageServerRequest : IUseCaseRequest<ManageServerResponse>
    {
        public Guid CustomerId { get; set; }
        public ICollection<Guid> ServerIds { get; set; }
        public ManageServerRequest(Guid customerId, ICollection<Guid> serverIds)
        {
            CustomerId = customerId;
            ServerIds = serverIds;
        }
    }
}
