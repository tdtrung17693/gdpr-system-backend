using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.GatewayResponses.Repositories;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface ICustomerRepository
    {
        public Task<IEnumerable<Object>> GetCustomerList();
        public Task<Customer> FindById(string id);
        public Task<ExportCSVByCustomerResponse> GetByCustomers(ExportCustomerRequest request);
        public Task<CRUDCustomerResponse> Create(Customer customer);
        public Task<CRUDCustomerResponse> Update(Customer customer);
        public Task<CRUDCustomerResponse> Delete(Customer customer);
    }
}
