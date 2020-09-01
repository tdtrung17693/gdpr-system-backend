using AutoMapper;
using System;
using Web.Api.Core.Domain.Entities;
using Web.Api.Infrastructure.Data.Entities;

using DomainEntities = Web.Api.Core.Domain.Entities;

namespace Web.Api.Infrastructure.Data.Mapping
{
    public class DataProfile : Profile
    {
        public DataProfile()
        {
            //CreateMap<User, AppUser>().ConstructUsing(u => new AppUser {Id=u.Id, FirstName = u.FirstName, LastName = u.LastName, UserName = u.UserName, PasswordHash = u.PasswordHash});
            //CreateMap<AppUser, User>().ConstructUsing(au => new User(au.FirstName, au.LastName, au.Email, au.UserName, au.Id, au.PasswordHash));
            CreateMap<Core.Domain.Entities.Customer, Entities.Customer>().ConstructUsing(s => new Entities.Customer { Id = s.Id, CreatedBy = s.CreatedBy, CreatedAt = s.CreatedAt, 
                UpdatedBy = s.UpdatedBy == null ? Guid.Empty : s.UpdatedBy, UpdatedAt = s.UpdatedAt, Name = s.Name, 
                ContactPoint = s.ContactPoint, ContractBeginDate = s.ContractBeginDate, ContractEndDate = s.ContractEndDate, 
                Status = s.Status, Description = s.Description });
            CreateMap<Entities.Customer, Core.Domain.Entities.Customer>().ConstructUsing(s_infrastruture => new Core.Domain.Entities.Customer(s_infrastruture.Id, s_infrastruture.CreatedBy, s_infrastruture.CreatedAt, s_infrastruture.UpdatedBy, s_infrastruture.UpdatedAt, 
                s_infrastruture.Name, s_infrastruture.ContractBeginDate, s_infrastruture.ContractEndDate, s_infrastruture.ContactPoint, s_infrastruture.Description, s_infrastruture.Status));
            CreateMap<DomainEntities.Account, DomainEntities.User>().ConstructUsing(acc =>
            {
                return new DomainEntities.User(
                    acc.User.Id,
                    acc.User.CreatedBy,
                    acc.User.CreatedAt,
                    acc.User.UpdatedBy,
                    acc.User.UpdatedAt,
                    acc.User.DeletedBy,
                    acc.User.DeletedAt,
                    acc.User.IsDeleted,
                    acc.User.FirstName,
                    acc.User.LastName,
                    acc.User.Email,
                    acc.User.RoleId);
                    //System.Text.Encoding.Default.GetString(acc.HashedPassword));
            });

        }
    }
}

