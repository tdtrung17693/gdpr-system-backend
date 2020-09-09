using AutoMapper;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.UseCaseResponses.User;

namespace Web.Api.Infrastructure.Data.Mapping
{
    public class DataProfile : Profile
    {
        public DataProfile()
        {
            CreateMap<User, UserDTO>().ConstructUsing(user =>
            {

              return new UserDTO
              {
                Id = user.Id.ToString(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Account.Username,
                RoleName = user.Role.Name,
                RoleId = user.RoleId.ToString(),
                Status = (bool)user.Status ? true : false
              };
                    //System.Text.Encoding.Default.GetString(acc.HashedPassword));
            });

        }
    }
}
