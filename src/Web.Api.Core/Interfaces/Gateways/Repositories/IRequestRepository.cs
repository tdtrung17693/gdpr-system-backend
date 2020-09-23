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
       
        
        Task<CreateRequestResponse> CreateRequest(Request request);
        Task<UpdateRequestResponse> UpdateRequest(Request request);
        Task<IList<ExportRequestDetail>> exportBulkRequest(Guid? uid, BulkExportRequest message);
        Task<IList<RequestDetail>> GetRequest(Guid? uid,int PageNo = 1, int PageSize = 10, string keyword = "", string filterStatus = "");
        Task<IList<ExportRequestDetail>> GetRequestForExport(ExportRequest request);
        RequestDetail getEachRequest(string requestId, string role);
        DataTable getNoRows(Guid? uid, string searchKey);
        Task<bool> ManageRequest(ManageRequestRequest message);
    }
}
