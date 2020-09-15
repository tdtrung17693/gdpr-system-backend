using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.GatewayResponses.Repositories;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface IServerRepository
    {
        //Todo: Read list server
       public IEnumerable<Server> GetAllCommand();

        public DataTable GetListServer();

        public DataTable GetListServerByFilter(string filter);

        //Todo:  Create a new server
        public Task<CRUDServerResponse> Create(Server server);

        //Todo: Update a server
        public Task<CRUDServerResponse> UpdateServer(Server server);

        //Todo: Delete a server
        public Task<CRUDServerResponse> DeleteServer(Server server);

        //Todo: get detail a server
        public Server GetServerDetail(Guid serverId);


        //Todo: update active and deactive multi server
        public Task<CRUDServerResponse> UpdateMutilServerStatus(DataTable serverIdList, Guid userId);

        //Todo: export files
        public Task<ExportCSVByCustomerResponse> GetExportServers(ExportServerRequest request);

        //Todo:Find By id
        public Task<Server> FindById(Guid id);

        //Todo:Find All
        public IPagedCollection<Server> FindAll();

        //Todo: Count Servers
        public DataTable CountServers();

        //Todo: Paging
        public DataTable Paging(int Page, int PageSize, string SortBy, bool SortOrder, string FilterBy);
    }
} 
