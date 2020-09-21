using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.GatewayResponses.Repositories;
using Web.Api.Core.Dto.UseCaseRequests.CustomerUseCaseRequest;
using Web.Api.Core.Dto.UseCaseResponses;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface ICustomerRepository
    {
        public Task<IEnumerable<Object>> GetCustomerList();
        public Task<Customer> FindById(string id);
        public Task<IEnumerable<Object>> Filter(string keyword);
        public Task<IEnumerable<Object>> GetAllServer();
        public Task<IEnumerable<Object>> FilterServer(string keyword);
        public Task<IEnumerable<Object>> GetOwnedServer(string id);
        public Task<IEnumerable<Object>> GetAvailableServer();
        public Task<IEnumerable<Object>> GetAllContactPoint();

        public Task<ExportCSVByCustomerResponse> GetByCustomers(ExportCustomerRequest request);
        public Task<CRUDCustomerResponse> CreateFromImport(CustomerRequest request);
        public Task<CRUDCustomerResponse> Create(Customer request);
        public Task<ManageServerCustomerResponse> AddServerOwner(ManageServerRequest request);
        public Task<ManageServerCustomerResponse> RemoveServerOwner(ManageServerRequest request);

        public Task<CRUDCustomerResponse> Update(Customer customer);
        public Task<CRUDCustomerResponse> Delete(Customer customer);
    }
}
