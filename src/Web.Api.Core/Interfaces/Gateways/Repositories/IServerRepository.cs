using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.GatewayResponses.Repositories;
using Web.Api.Core.Dto.UseCaseResponses;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface IServerRepository
    {
        //Todo: Read list server
       IEnumerable<Server> GetAllCommand();

        //Todo:  Create a new server
       Task<CRUDServerResponse> Create(Server server);

        //Todo: Update a server
        Task<CRUDServerResponse> UpdateServer(Server server);

        //Todo: Delete a server
        Task<CRUDServerResponse> DeleteServer(Server server);

        //Todo: get detail a server
        Server GetServerDetail(Guid serverId);
        
        //Todo: update active and deactive multi server
        Task<CRUDServerResponse> UpdateMutilServerStatus(DataTable serverIdList, bool status, Guid userId);
    }
} 
