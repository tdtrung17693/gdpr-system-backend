using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Domain.Event;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.GatewayResponses.Repositories;
using Web.Api.Core.Dto.Requests;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface ILogRepository
    {
        public Task<DataTable> Create(CreateLog logMessage, User creator);
        public Task<DataTable> LogNewRequest(Guid requestId, User creator);

        public Task LogUpdateRequest(Guid requestId, Dictionary<string, List<string>> updatedFields,
            User updator);
        public Task LogAcceptRejectRequest(Guid requestId, User updator, string newRequestStatus);

        public Task<DataTable> GetListLogOfRequest(Guid requestId);


    }
}
