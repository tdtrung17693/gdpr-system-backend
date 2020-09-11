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
using System.Collections.Generic;
using Web.Api.Infrastructure.Helpers;
using PasswordGenerator;
using Web.Api.Core.Interfaces.Services.Event;
using Web.Api.Core.Domain.Event;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Web.Api.Infrastructure.Data.EntityFramework.Repositories
{
  internal sealed class UserRepository : IUserRepository
  {
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly IDomainEventBus _eventBus;

    public UserRepository(ApplicationDbContext context, IMapper mapper, IDomainEventBus eventBus)
    {
      _mapper = mapper;
      _context = context;
      _eventBus = eventBus;
    }

    public async Task<CreateUserResponse> Create(User userInfo, string userName, string password, Guid creator)
    {
      //var appUser = _mapper.Map<AppUser>(user);
      //var identityResult = await _userManager.CreateAsync(appUser, password);
      //return new CreateUserResponse(appUser.Id, identityResult.Succeeded, identityResult.Succeeded ? null : identityResult.Errors.Select(e => new Error(e.Code, e.Description)));
      var salt = GenerateSalt(10);
      var rawPassword = new Password().IncludeLowercase().IncludeNumeric().IncludeUppercase().IncludeSpecial().Next();
      var hashPassword = CalculateHash(rawPassword, salt);

      var command = _context.Database.GetDbConnection().CreateCommand();
      command.CommandText = "CreateUser";
      command.CommandType = CommandType.StoredProcedure;
      command.Parameters.Add(new SqlParameter("@FirstName", userInfo.FirstName));
      command.Parameters.Add(new SqlParameter("@LastName", userInfo.LastName));
      command.Parameters.Add(new SqlParameter("@Email", userInfo.Email));
      command.Parameters.Add(new SqlParameter("@Username", userName));
      command.Parameters.Add(new SqlParameter("@HashedPassword", hashPassword));
      command.Parameters.Add(new SqlParameter("@Salt", Convert.ToBase64String(salt)));
      command.Parameters.Add(new SqlParameter("@RoleId", userInfo.RoleId));
      command.Parameters.Add(new SqlParameter("@Creator", creator));
      command.Parameters.Add(new SqlParameter("@Status", 1));

      if (command.Connection.State == ConnectionState.Closed)
        command.Connection.Open();
      try
      {
        var result = (Guid)await command.ExecuteScalarAsync();
        await _eventBus.Trigger(new UserCreated(userInfo.FirstName, userInfo.LastName, rawPassword, userInfo.Email, userName));
        return new CreateUserResponse(result.ToString(), true, null);
      }
      catch (SqlException e)
      {
        // Unique constraint violation code number
        if (e.Number == 2627)
          return new CreateUserResponse("-1", false, new[] { new Error(Error.Codes.UNIQUE_CONSTRAINT_VIOLATED, Error.Messages.UNIQUE_CONSTRAINT_VIOLATED) });
      }
      finally
      {
        command.Connection.Close();
      }

      return new CreateUserResponse("-1", false, new[] { new Error(Error.Codes.UNKNOWN, Error.Messages.UNKNOWN), });
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

    public async Task<User> FindById(Guid id)
    {
      User user = await _context.User
                                      .Include(u => u.Account)
                                      .Include(u => u.Role)
                                      .Where(u => u.Id == id)
                                      .FirstOrDefaultAsync();
      return user;
    }

    public async Task<User> FindByEmail(string email)
    {
      User user = await _context.User
                                      .Include(u => u.Account)
                                      .Include(u => u.Role)
                                      .Where(u => u.Email == email)
                                      .FirstOrDefaultAsync();
      return user;
    }

    public async Task<bool> CheckPassword(User user, string password)
    {
      //return await _userManager.CheckPasswordAsync(_mapper.Map<AppDataEntities.User>(user), password);
      var hashedPassword = Convert.ToBase64String(user.Account.HashedPassword);
      Console.Out.WriteLine(hashedPassword);
      var salt = Convert.FromBase64String(user.Account.Salt);
      try
      {
        var bytes = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA512, 10000, 32);
        var inputPassword = Convert.ToBase64String(bytes);
        return hashedPassword.Equals(inputPassword);
      }
      catch
      {
        return false;
      }
    }

    public byte[] CalculateHash(string input, byte[] salt)
    {
      var bytes = KeyDerivation.Pbkdf2(input, salt, KeyDerivationPrf.HMACSHA512, 10000, 32);

      return bytes;
    }

    private static byte[] GenerateSalt(int length)
    {
      var salt = new byte[length];

      using (var random = RandomNumberGenerator.Create())
      {
        random.GetBytes(salt);
      }

      return salt;
    }

    public IPagedCollection<User> FindAll()
    {
      // TODO: Remove magic number
      return new PagedCollection<User>(
          _context.User
            .Include(user => user.Account)
            .Include(user => user.Role)
            .AsQueryable()
      );
    }

    public async Task<UpdateUserResponse> Update(Guid id, string firstName, string lastName)
    {
      var user = await _context.User.Where(u => u.Id == id).FirstOrDefaultAsync();
      var updatedFields = new Dictionary<string, string>();
      if (user.FirstName != firstName) {
        updatedFields.Add("FirstName", firstName);
        user.FirstName = firstName;
      }

      if (user.LastName != lastName) {
        updatedFields.Add("LastName", lastName);
        user.LastName = lastName;
      }
      if (updatedFields.Count() > 0)
      {
        try
        {
          await _context.SaveChangesAsync();
        } catch
        {
          return new UpdateUserResponse(false, new[] { new Error(Error.Codes.UNKNOWN, Error.Messages.UNKNOWN) });
        }
      }
      return new UpdateUserResponse(updatedFields);
    }
    public async Task<UpdateUserResponse> Update(Guid id, Guid roleId, bool status)
    {
      var user = await _context.User.Where(u => u.Id == id).FirstOrDefaultAsync();
      var updatedFields = new Dictionary<string, string>();

      if (user == null)
      {
        return new UpdateUserResponse(false, new[] { new Error(Error.Codes.ENTITY_NOT_FOUND, Error.Messages.ENTITY_NOT_FOUND)});
      }

      if (user.RoleId != roleId)
      {
        updatedFields.Add("RoleId", roleId.ToString());
        user.RoleId = roleId;
      }
      if (user.Status != status)
      {
        updatedFields.Add("Status", status.ToString());
        user.Status = status;
      }

      if (updatedFields.Count() > 0)
      {
        try
        {
          await _context.SaveChangesAsync();
        }
        catch
        {
          return new UpdateUserResponse(false, new[] { new Error(Error.Codes.UNKNOWN, Error.Messages.UNKNOWN) });
        }
      }

      return new UpdateUserResponse(updatedFields);
    }

    public Task<CreateUserResponse> Delete(User user)
    {
      throw new NotImplementedException();
    }

    public async Task<UpdateUserResponse> ChangeStatus(ICollection<Guid> ids, bool status)
    {
      List<User> userList = await _context.User.Where(u => ids.Contains((Guid)u.Id)).ToListAsync();
      userList.ForEach(u => u.Status = status);
      try
      {
        await _context.SaveChangesAsync();
      } catch (Exception e)
      {
        return new UpdateUserResponse(false, new[] { new Error(Error.Codes.UNKNOWN, Error.Messages.UNKNOWN) });
      }

      return new UpdateUserResponse(true);
    }
  }
}
