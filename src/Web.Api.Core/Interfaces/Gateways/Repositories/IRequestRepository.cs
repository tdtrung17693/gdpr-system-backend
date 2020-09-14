using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.GatewayResponses.Repositories;
using System.Data;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Dto.UseCaseRequests;
using CreateRequestResponse = Web.Api.Core.Dto.GatewayResponses.Repositories.CreateRequestResponse;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface IRequestRepository
    {
        IEnumerable<Request> GetRequestList();
        Task<CRUDRequestResponse> Create(Request request);
        Task<CRUDRequestResponse> Update(Request request);
        Task<CRUDRequestResponse> Delete(Request request);
        Task<CRUDRequestResponse> UpdateBulkRequestStatus(DataTable requestIdList, bool status, Guid userId);
        Task<CreateRequestResponse> CreateRequest(Request request);
        Task<UpdateRequestResponse> UpdateRequest(Request request);
        Task<IList<RequestDetail>> GetRequest(int PageNo, int PageSize);
        //IList<RequestJoined> GetRequestFilter(string keyword, int pageNo, int pageSize);
        Task<int> getNoPages(int PageSize);
    }
}
