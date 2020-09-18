using System.Linq;
using System.Threading.Tasks;
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
using PasswordGenerator;
using Web.Api.Core.Interfaces.Services.Event;
using Web.Api.Core.Domain.Event;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Web.Api.Core.Dto.UseCaseRequests;
using System.IO;

namespace Web.Api.Infrastructure.Data.EntityFramework.Repositories
{
    internal sealed class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IDomainEventBus _eventBus;

        public UserRepository(ApplicationDbContext context, IDomainEventBus eventBus)
        {
            _context = context;
            _eventBus = eventBus;
        }

        public async Task<CreateUserResponse> Create(User userInfo, string userName, string password, Guid creator)
        {
            var salt = GenerateSalt(10);
            var rawPassword = new Password().IncludeLowercase().IncludeNumeric().IncludeUppercase().IncludeSpecial()
                .Next();
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
                await command.Connection.OpenAsync();
            try
            {
                var result = (Guid) await command.ExecuteScalarAsync();
                _eventBus.Trigger(new UserCreated(userInfo.FirstName, userInfo.LastName, rawPassword,
                    userInfo.Email, userName));
                return new CreateUserResponse(result.ToString(), true);
            }
            catch (SqlException e)
            {
                // Unique constraint violation code number
                if (e.Number == 2627)
                    return new CreateUserResponse(new[]
                        {new Error(Error.Codes.UNIQUE_CONSTRAINT_VIOLATED, Error.Messages.UNIQUE_CONSTRAINT_VIOLATED)});
            }

            return new CreateUserResponse(new[] {new Error(Error.Codes.UNKNOWN, Error.Messages.UNKNOWN),});
        }

        public async Task<User> FindByName(string userName)
        {
            //return _mapper.Map<DataEntities.User>(await _userManager.FindByNameAsync(userName));
            var user = await _context.User
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
            var user = await _context.User
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

        private static byte[] CalculateHash(string input, byte[] salt)
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

    public async Task<UpdateUserResponse> UpdateProfileInfo(User user, string firstName, string lastName)
    {
      var updatedFields = new Dictionary<string, string>();
      if (user.FirstName != firstName) {
        updatedFields.Add("FirstName", firstName);
        user.FirstName = firstName;
      }

      if (user.LastName != lastName) {
        updatedFields.Add("LastName", lastName);
        user.LastName = lastName;
      }
      if (updatedFields.Any())
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

    public async Task<UpdateUserResponse> ChangePassword(User user, string newPassword)
    {
      var salt = GenerateSalt(10);
      var hashPassword = CalculateHash(newPassword, salt);
      user.Account.HashedPassword = hashPassword;
      user.Account.Salt = Convert.ToBase64String(salt);
      
      try
      {
        await _context.SaveChangesAsync();
      }
      catch (Exception e)
      {
        return new UpdateUserResponse(false, new[] { new Error(Error.Codes.UNKNOWN, Error.Messages.UNKNOWN) });
      }

      return new UpdateUserResponse(true);
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

        //Cho nay cua em nha a Trung :D
     public async Task<Object> GetAvatar(string id)
        {
            var fileInfo = await _context.FileInstance.AsNoTracking()
               .FirstOrDefaultAsync(fi => fi.UserFileInstance.Any(cs => cs.UserId == Guid.Parse(id)));

            if (fileInfo == null) return null;
            Byte[] fileBytes = File.ReadAllBytes(Path.Combine(fileInfo.Path, fileInfo.FileName + "." + fileInfo.Extension));
            String content = Convert.ToBase64String(fileBytes);
            return new
            {
                id = fileInfo.Id,
                fileName = fileInfo.FileName,
                fileExtension = fileInfo.Extension,
                content,
            };   
     }

     public async Task<UploadAvatarUserResponse> UploadFirstAvatar(UploadAvatarRequest request)
     {
        byte[] imageBytes = Convert.FromBase64String(request.Content);

        //Save the Byte Array as Image File.
        string path = Directory.GetParent(Environment.CurrentDirectory).FullName;
        string fileDirectory = Path.Combine(path, "Web.Api\\FileInstance");
        string filePath = Path.Combine(fileDirectory, request.FileName + "." +  request.FileExtension);
        File.WriteAllBytes(filePath, imageBytes);
        Guid fileId = Guid.NewGuid();

        //Save to database
        await _context.FileInstance.AddAsync(new FileInstance(fileId, request.FileName, request.FileExtension, fileDirectory));
        await _context.UserFileInstance.AddAsync(new UserFileInstance(request.UserId, fileId));
        var success = await _context.SaveChangesAsync();
        return new UploadAvatarUserResponse(fileId, success > 0, null);       
     }   

     public async Task<UploadAvatarUserResponse> ChangeAvatar(UploadAvatarRequest request)
     {
        byte[] imageBytes = Convert.FromBase64String(request.Content);

        //Save the Byte Array as Image File.
        string path = Directory.GetParent(Environment.CurrentDirectory).FullName;
        string fileDirectory = Path.Combine(path, "Web.Api\\FileInstance");
        string filePath = Path.Combine(fileDirectory, request.FileName + "." +  request.FileExtension);
        File.WriteAllBytes(filePath, imageBytes);

        var fileId = request.FileId;
        //await _context.FileInstance.AddAsync(new FileInstance((Guid)fileId, request.FileName, request.FileExtension, "Some path" ));
        var updatedFileInstance = await _context.FileInstance.FirstOrDefaultAsync(i => i.Id == fileId);
            if (updatedFileInstance != null)
            {
                _context.Attach(updatedFileInstance);
                updatedFileInstance.FileName = request.FileName;
                updatedFileInstance.Extension = request.FileExtension;
            }
        var success = await _context.SaveChangesAsync();
        return new UploadAvatarUserResponse(success > 0, null);       
     }   
  }
}