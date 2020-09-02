using AutoMapper;
using DomainEntities = Web.Api.Core.Domain.Entities;

namespace Web.Api.Infrastructure.Data.Mapping
{
    public class DataProfile : Profile
    {
        public DataProfile()
        {
            CreateMap<DomainEntities.Account, DomainEntities.User>().ConstructUsing(acc =>
            {
                return new DomainEntities.User(
                    acc.User.FirstName,
                    acc.User.LastName,
                    acc.User.Email,
                    acc.User.RoleId);
                    //System.Text.Encoding.Default.GetString(acc.HashedPassword));
            });

        }
    }
}

// == null?Guid.Empty:s.DeletedBy