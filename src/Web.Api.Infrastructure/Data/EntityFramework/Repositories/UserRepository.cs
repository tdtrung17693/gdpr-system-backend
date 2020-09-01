using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Web.Api.Core;
using DataEntities = Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.GatewayResponses.Repositories;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using System;

namespace Web.Api.Infrastructure.Data.EntityFramework.Repositories
{
    internal sealed class UserRepository : IUserRepository
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<CreateUserResponse> Create(DataEntities.User user, string password)
        {
            //var appUser = _mapper.Map<AppUser>(user);
            //var identityResult = await _userManager.CreateAsync(appUser, password);
            //return new CreateUserResponse(appUser.Id, identityResult.Succeeded, identityResult.Succeeded ? null : identityResult.Errors.Select(e => new Error(e.Code, e.Description)));
            return default(CreateUserResponse);
        }

        public async Task<DataEntities.User> FindByName(string userName)
        {
            //return _mapper.Map<DataEntities.User>(await _userManager.FindByNameAsync(userName));
            return default(DataEntities.User);
        }
        public async Task<DataEntities.User> FindById(string id)
        {
            Guid userId = Guid.Parse(id);
            DataEntities.User user = await _context.User
                                            .Where(u => u.Id == userId)
                                            .Include("Account")
                                            .FirstAsync();
            return _mapper.Map<DataEntities.User>(user.Account);
        }

        public async Task<bool> CheckPassword(DataEntities.User user, string password)
        {
            //return await _userManager.CheckPasswordAsync(_mapper.Map<AppDataEntities.User>(user), password);
            return default(bool);
        }

        public IPagedCollection<DataEntities.User> FindAll()
        {
            // TODO: Remove magic number
            return new PagedCollection<DataEntities.User, DataEntities.Account>(
                _context.Account.Include("User").AsQueryable(),
                _mapper,
                10
            );
        }
    }
}
