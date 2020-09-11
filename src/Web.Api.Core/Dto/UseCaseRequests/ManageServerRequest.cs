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
        public bool Action { get; set; } // 1 for Insert and 0 for Delete
        public ManageServerRequest(Guid customerId, ICollection<Guid> serverIds, bool action)
        {
            CustomerId = customerId;
            ServerIds = serverIds;
            Action = action;
        }
    }
}
