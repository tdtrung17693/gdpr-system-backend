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
using Web.Api.Core.Dto.UseCaseResponses;
using System.Data.SqlClient;
using System.Data;

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

        public async Task<IEnumerable<Object>> GetCustomerList()
        {
            return await _context.Customer.AsNoTracking()
                .Select(c => new { key = c.Id, c.Name, c.ContractBeginDate, c.ContractEndDate, c.Description, c.Status, contactPoint = c.ContactPointNavigation.Email,
                serverOwned = c.CustomerServer.Select(cs => new { cs.Server.Id, cs.Server.Name, cs.Server.IpAddress }).Count()
                }).ToListAsync();
        }

        public async Task<Customer> FindById(string id)
        {
            return await _context.Customer.AsNoTracking().Include(c => c.CustomerServer).ThenInclude(cs => cs.Server).FirstOrDefaultAsync(i => i.Id == Guid.Parse(id));
        }

        public async Task<IEnumerable<Object>> Filter(string keyword)
        {
            return await _context.Customer.AsNoTracking()
                .Select(c => new { key = c.Id, c.Name, c.ContractBeginDate, c.ContractEndDate, c.Description, c.Status, contactPoint = c.ContactPointNavigation.Email,
                serverOwned = c.CustomerServer.Select(cs => new { cs.Server.Id, cs.Server.Name, cs.Server.IpAddress }).Count()
                })
                .Where(c => c.Name.Contains(keyword) || c.Description.Contains(keyword)).ToListAsync();
        }

        public async Task<IEnumerable<Object>> GetAllServer()
        {
            return await _context.Server.AsNoTracking()
                .Select(c => new
                {
                    id = c.Id,
                    c.Name,
                    c.IpAddress,
                    ownedBy = c.CustomerServer.Select(cs => cs.Customer.Name),
                    customerid = c.CustomerServer.Select(cs => cs.Customer.Id),
                }).Take(50).ToListAsync();
        }

        public async Task<IEnumerable<Object>> FilterServer(string keyword)
        {
            return await _context.Server.AsNoTracking()
                .Select(c => new {
                    c.Id,
                    c.Name,
                    c.IpAddress,
                    ownedBy = c.CustomerServer.Select(cs => cs.Customer.Name),
                    customerid = c.CustomerServer.Select(cs => cs.Customer.Id),
                })
                .Where(c => c.Name.Contains(keyword) || c.IpAddress.Contains(keyword)).ToListAsync();
        }

        public async Task<IEnumerable<Object>> GetOwnedServer(string id)
        {
            return await _context.Server.AsNoTracking()
                .Where(s => s.CustomerServer.Any(cs => cs.Customer.Id == Guid.Parse(id)))
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.IpAddress,
                    ownedBy = s.CustomerServer.Select(cs => cs.Customer.Name),
                    customerid = s.CustomerServer.Select(cs => cs.Customer.Id),
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<Object>> GetAvailableServer()
        {
            return await _context.Server.AsNoTracking()
                .Where(s => s.CustomerServer.Count() == 0)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.IpAddress,
                    ownedBy = s.CustomerServer.Select(cs => cs.Customer.Name),
                    customerid = s.CustomerServer.Select(cs => cs.Customer.Id),
                })
                .Take(156)
                .ToListAsync();
        }

        public async Task<IEnumerable<Object>> GetAllContactPoint()
        {
            return await _context.User.AsNoTracking()
                .Where(c => c.Role.Name == "Contact Point" || c.Role.Name == "Administrator")
                .Select(c => new
                {
                    id = c.Id,
                    c.Email,
                }).ToListAsync();
        }

        public async Task<CRUDCustomerResponse> Create(Customer /*CustomerRequest*/ customer)
        {
            await _context.Customer.AddAsync(customer);
            var success = await _context.SaveChangesAsync();
            /*return new CreateCustomerResponse(newCustomer.Id, success > 0, 
                success > 0 ? null : new IdentityError(){Description = $"Could not add user {customer.Id}."});*/
            return new CRUDCustomerResponse(customer.Id, success > 0,
                null);
        }

        /*public async Task<ManageServerCustomerResponse> AddServerOwner(ManageServerRequest request)
        {
            var customerId = new SqlParameter("@CustomerId", request.CustomerId);
            var serverIds = new SqlParameter("@ServerIds", request.ServerIds);
            serverIds.SqlDbType = SqlDbType.Structured;
            serverIds.TypeName = "dbo.IdList";
            //Console.WriteLine(request.ServerIds);
            *//*_context.Database.ExecuteSqlCommand("DECLARE @si IdList INSERT @si VALUES('390EA8C0-1714-415F-B4AD-00447E8A7F2D')" +
               "EXEC[gdpr_system].[dbo].[AssignCustomerToServers] @CustomerId, @ServerIds = @si", customerId);*//*
            _context.Database.ExecuteSqlCommand("EXEC dbo.AssignCustomerToServers @CustomerId, @ServerIds", customerId, serverIds);

            var success = await _context.SaveChangesAsync();
            return new ManageServerCustomerResponse(success > 0,
                null);
        }*/
        public async Task<ManageServerCustomerResponse> AddServerOwner(ManageServerRequest request)
        {
            foreach (var serverId in request.ServerIds)
            {
                await _context.CustomerServer.AddAsync(new CustomerServer(request.CustomerId, serverId));
            }
            var success = await _context.SaveChangesAsync();
            return new ManageServerCustomerResponse(success > 0,
                null);
        }

        public async Task<ManageServerCustomerResponse> RemoveServerOwner(ManageServerRequest request)
        {
            foreach (var serverId in request.ServerIds)
            {
                var deletedCustomerServer = await _context.CustomerServer.FindAsync(request.CustomerId, serverId);
                _context.CustomerServer.Remove(deletedCustomerServer);
                //await _context.CustomerServer.Remove(new CustomerServer(request.CustomerId, serverId));
            }
            var success = await _context.SaveChangesAsync();
            return new ManageServerCustomerResponse(success > 0,
                null);
        }

        public async Task<ExportCSVByCustomerResponse> GetByCustomers(ExportCustomerRequest request)
        {
            var response = await _context.Server.AsNoTracking()
                .Where(s => s.CustomerServer.Any(cs => request.Guids.Contains(cs.Customer.Id)))
                .Where(s => s.Request.Any(r => (r.EndDate <= request.ToDate && r.StartDate >= request.FromDate && r.ApprovedBy != null)))
                .Select(s => new {
                    Request = s.Request
                .Where(r => r.ApprovedBy != null)
                .Select(r => new { s.Id, s.Name, s.IpAddress, r.Title, r.StartDate, r.EndDate, Requester = r.CreatedByNavigation.Email, Approver = r.ApprovedByNavigation.Email })
                })
                .ToListAsync();
            return new ExportCSVByCustomerResponse(response, true, null);
        }

        public async Task<CRUDCustomerResponse> Update(Customer /*CustomerRequest*/ customer)
        {
            //var updatedCustomer = _context.Entry(await _context.Customer.FirstOrDefaultAsync(i => i.Id == customer.Id));
            var updatedCustomer = await _context.Customer.FirstOrDefaultAsync(i => i.Id == customer.Id);
            if (updatedCustomer != null)
            {
                _context.Attach(updatedCustomer);
                updatedCustomer.Description = customer.Description;
                updatedCustomer.Name = customer.Name;
                updatedCustomer.ContractBeginDate = customer.ContractBeginDate;
                updatedCustomer.ContractEndDate = customer.ContractEndDate;
                updatedCustomer.Status = customer.Status;
                updatedCustomer.ContactPoint = customer.ContactPoint;
            }
           /* _context.Customer.Remove(oldCustomer);
            await _context.SaveChangesAsync();
            await _context.Customer.AddAsync(customer);*/
            var success = await _context.SaveChangesAsync();
            return new CRUDCustomerResponse(customer.Id, success > 0,
                null);
        }

        public async Task<CRUDCustomerResponse> Delete(Customer /*CustomerRequest*/ customer)
        {
            var deletedCustomer = await _context.Customer.FindAsync(customer.Id);
            _context.Customer.Remove(deletedCustomer);
            var success = await _context.SaveChangesAsync();
            return new CRUDCustomerResponse(customer.Id, success > 0,
                null);
        }   
    }
}
