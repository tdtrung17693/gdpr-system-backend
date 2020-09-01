using System;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using Web.Api.Core;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Dto.GatewayResponses.Repositories;

using Web.Api.Core.Domain.Entities;
using System.Data.SqlClient;
using System.Data;
using Web.Api.Core.Dto;

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
            return default(User);
        }
        public async Task<User> FindById(string id)
        {
            Guid userId = Guid.Parse(id);
            User user = await _context.User
                                            .Where(u => u.Id == userId)
                                            .Include("Account")
                                            .FirstAsync();
            return _mapper.Map<User>(user.Account);
        }

        public async Task<bool> CheckPassword(User user, string password)
        {
            //return await _userManager.CheckPasswordAsync(_mapper.Map<AppDataEntities.User>(user), password);
            return default(bool);
        }

        public IPagedCollection<User> FindAll()
        {
            // TODO: Remove magic number
            return new PagedCollection<User, Account>(
                _context.Account.Include("User").AsQueryable(),
                _mapper,
                10
            );
        }
    }
}
