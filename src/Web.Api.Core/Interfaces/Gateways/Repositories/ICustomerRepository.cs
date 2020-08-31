using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.GatewayResponses.Repositories;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface ICustomerRepository
    {
        public IEnumerable<Customer> GetCustomerList();
        public Task<CRUDCustomerResponse> Create(Customer customer);
        public Task<CRUDCustomerResponse> Update(Customer customer);
        public Task<CRUDCustomerResponse> Delete(Customer customer);
    }
    /*public interface ICreateForAsync<T> where T : class
    {
        Task<OperationResult> CreateAsync(T entityToCreate);
    }*/
}
