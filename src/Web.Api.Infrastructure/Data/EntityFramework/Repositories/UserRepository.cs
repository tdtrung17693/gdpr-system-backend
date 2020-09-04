using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Web.Api.Core;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Dto.GatewayResponses.Repositories;

using Web.Api.Core.Domain.Entities;
using System.Data.SqlClient;
using System.Data;
using Web.Api.Core.Dto;
using DataEntities = Web.Api.Core.Domain.Entities;
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

        public async Task<CreateUserResponse> Create(User user, string password)
        {
            //var appUser = _mapper.Map<AppUser>(user);
            //var identityResult = await _userManager.CreateAsync(appUser, password);
            //return new CreateUserResponse(appUser.Id, identityResult.Succeeded, identityResult.Succeeded ? null : identityResult.Errors.Select(e => new Error(e.Code, e.Description)));
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "CreateUser";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@FirstName", user.FirstName));
            command.Parameters.Add(new SqlParameter("@LastName", user.LastName));
            command.Parameters.Add(new SqlParameter("@Email", user.Email));
            command.Parameters.Add(new SqlParameter("@Username", user.Account.Username));
            command.Parameters.Add(new SqlParameter("@HashedPassword", hashPassword));
            command.Parameters.Add(new SqlParameter("@Salt", salt));

            if (command.Connection.State == ConnectionState.Closed)
                command.Connection.Open();
            try
            {
                var result = (Guid)await command.ExecuteScalarAsync();
                Console.Out.WriteLine(result);
                return new CreateUserResponse(result.ToString(), true, null);
            }
            catch (Exception e)
            {
                return new CreateUserResponse("-1", false, new[] { new Error(null, e.Message), }) ;
            }
            finally
            {
                command.Connection.Close();
            }
        }

        public async Task<User> FindByName(string userName)
        {
            //return _mapper.Map<DataEntities.User>(await _userManager.FindByNameAsync(userName));
            DataEntities.User user = await _context.User
                                            .Include(u => u.Account)
                                            .Include(u => u.Role)
                                            .Where(u => u.Account.Username == userName)
                                            .FirstOrDefaultAsync();
            return user;
        }
        public async Task<User> FindById(string id)
        {
            Guid userId = Guid.Parse(id);
            User user = await _context.User
                                            .Include(u => u.Account)
                                            .Include(u => u.Role)
                                            .Where(u => u.Id == userId)
                                            .FirstOrDefaultAsync();
            return user;
        }

        public async Task<bool> CheckPassword(User user, string password)
        {
            //return await _userManager.CheckPasswordAsync(_mapper.Map<AppDataEntities.User>(user), password);
            Console.Out.WriteLine(System.Text.Encoding.Default.GetString(user.Account.HashedPassword));
            return BCrypt.Net.BCrypt.Verify(password, System.Text.Encoding.Default.GetString(user.Account.HashedPassword));
        }

        public IPagedCollection<User> FindAll()
        {
            // TODO: Remove magic number
            return new PagedCollection<DataEntities.User, DataEntities.Account>(
                _context.Account.Include(account => account.User).ThenInclude(user => user.Role).AsQueryable(),
                _mapper,
                10
            );
        }

    public Task<CreateUserResponse> Update(User user)
    {
      throw new NotImplementedException();
    }

    public Task<CreateUserResponse> Delete(User user)
    {
      throw new NotImplementedException();
    }
  }
}
