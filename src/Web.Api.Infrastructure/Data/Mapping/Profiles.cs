using AutoMapper;
using DomainEntities = Web.Api.Core.Domain.Entities;
using DataEntities = Web.Api.Infrastructure.Data.EntityFramework.Entities;

namespace Web.Api.Infrastructure.Data.Mapping
{
    public class DataProfile : Profile
    {
        public DataProfile()
        {
            CreateMap<DataEntities.Account, DomainEntities.User>().ConstructUsing(acc =>
            {
                return new DomainEntities.User(
                    acc.User.FirstName,
                    acc.User.LastName, 
                    acc.User.Email,
                    acc.Username,
                    acc.User.Id.ToString(),
                    System.Text.Encoding.Default.GetString(acc.HashedPassword));
            });

        }
    }
}
