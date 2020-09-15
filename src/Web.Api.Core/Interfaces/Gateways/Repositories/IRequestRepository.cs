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
        
        Task<CreateRequestResponse> CreateRequest(Request request);
        Task<UpdateRequestResponse> UpdateRequest(Request request);
        Task<IList<RequestDetail>> GetRequest(int PageNo = 1, int PageSize = 10, string keyword = "", string filterStatus = ""/*, DateTime? FromDateExport = null, DateTime? TSoDateExport = null*/);
        Task<IList<RequestDetail>> GetRequestForExport(ExportRequest request);
        Task<int> getNoPages(int PageSize);
    }
}
