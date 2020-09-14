using AutoMapper;
using System;
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
                    acc.User.RoleId,
                    new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                    new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                    DateTime.UtcNow);
                    //System.Text.Encoding.Default.GetString(acc.HashedPassword));
            });

        }
    }
}
