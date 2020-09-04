using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.GatewayResponses.Repositories;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Microsoft.EntityFrameworkCore;
using Web.Api.Core.Dto.UseCaseRequests;

namespace Web.Api.Infrastructure.Data.EntityFramework.Repositories
{
    internal sealed class CustomerRepository : ICustomerRepository
    {
        public readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public CustomerRepository(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetCustomerList()
        {
            return await _context.Customer.AsNoTracking().Include(c => c.CustomerServer).ThenInclude(cs => cs.Server).ToListAsync();
        }

        public async Task<Customer> FindById(string id)
        {
            return await _context.Customer.AsNoTracking().Include(c => c.CustomerServer).ThenInclude(cs => cs.Server).FirstOrDefaultAsync(i => i.Id == Guid.Parse(id));
        }
        public async Task<CRUDCustomerResponse> Create(Customer customer)
        {
            //var newCustomer = _mapper.Map<Customer>(customer);
            await _context.Customer.AddAsync(customer);
            var success = await _context.SaveChangesAsync();
            /*return new CreateCustomerResponse(newCustomer.Id, success > 0, 
                success > 0 ? null : new IdentityError(){Description = $"Could not add user {customer.Id}."});*/
            return new CRUDCustomerResponse(customer.Id, success > 0,
                null);
        }

        public async Task<CRUDCustomerResponse> Update(Customer customer)
        {
            var oldCustomer = await _context.Customer.FindAsync(customer.Id);
            _context.Customer.Remove(oldCustomer);
            await _context.SaveChangesAsync();
            await _context.Customer.AddAsync(customer);
            var success = await _context.SaveChangesAsync();
            return new CRUDCustomerResponse(customer.Id, success > 0,
                null);
        }

        public async Task<CRUDCustomerResponse> Delete(Customer customer)
        {
            var deletedCustomer = await _context.Customer.FindAsync(customer.Id);
            _context.Customer.Remove(deletedCustomer);
            var success = await _context.SaveChangesAsync();
            return new CRUDCustomerResponse(customer.Id, success > 0,
                null);
        }   
    }
}
