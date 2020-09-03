using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.GatewayResponses.Repositories;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface IRequestRepository
    {
        IEnumerable<Request> GetRequestList();
        Task<CRUDRequestResponse> Create(Request request);
        Task<CRUDRequestResponse> Update(Request request);
        Task<CRUDRequestResponse> Delete(Request request);
    }
}