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

namespace Web.Api.Infrastructure.Data.EntityFramework.Repositories
{
    internal sealed class CustomerRepository: ICustomerRepository
    {
        public readonly IMapper _mapper;
        private readonly GdprContext _context;

        public CustomerRepository(IMapper mapper, GdprContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public IEnumerable<Customer> GetCustomerList()
        {
            return _context.Customer.ToList();
        }

        public async Task<CreateCustomerResponse> Create(Customer customer)
        {
            var newCustomer = _mapper.Map<Customer>(customer);
            await _context.Customer.AddAsync(newCustomer);
            var success = await _context.SaveChangesAsync();
            /*return new CreateCustomerResponse(newCustomer.Id, success > 0, 
                success > 0 ? null : new IdentityError(){Description = $"Could not add user {customer.Id}."});*/
            return new CreateCustomerResponse(newCustomer.Id, success > 0,
                null);
        }
    }
}
