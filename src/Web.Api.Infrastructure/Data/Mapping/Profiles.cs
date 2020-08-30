using AutoMapper;
using System;
using System.Collections.Generic;
using Web.Api.Core.Domain.Entities;
using Web.Api.Infrastructure.Data.Entities;


namespace Web.Api.Infrastructure.Data.Mapping
{
    public class DataProfile : Profile
    {
        public DataProfile()
        {
            //CreateMap<User, AppUser>().ConstructUsing(u => new AppUser {Id=u.Id, FirstName = u.FirstName, LastName = u.LastName, UserName = u.UserName, PasswordHash = u.PasswordHash});
            //CreateMap<AppUser, User>().ConstructUsing(au => new User(au.FirstName, au.LastName, au.Email, au.UserName, au.Id, au.PasswordHash));
            CreateMap<Core.Domain.Entities.Server, Entities.Server>().ConstructUsing(s => new Entities.Server {Id = s.Id, CreatedBy = s.CreatedBy, CreatedAt = s.CreatedAt, UpdatedBy = s.UpdatedBy == null ? Guid.Empty : s.UpdatedBy, UpdatedAt = s.UpdatedAt, DeletedBy = s.DeletedBy == null ? Guid.Empty : s.DeletedBy, DeletedAt = s.DeletedAt,  isDeleted = s.isDeleted, Name = s.Name, IpAddress = s.IpAddress, StartDate = s.StartDate, EndDate = s.EndDate });
            CreateMap<Entities.Server, Core.Domain.Entities.Server>().ConstructUsing(s_infrastruture => new Core.Domain.Entities.Server(s_infrastruture.Id, s_infrastruture.CreatedBy, s_infrastruture.CreatedAt, s_infrastruture.UpdatedBy, s_infrastruture.UpdatedAt , s_infrastruture.DeletedBy, s_infrastruture.DeletedAt, s_infrastruture.isDeleted, s_infrastruture.Name, s_infrastruture.IpAddress, s_infrastruture.StartDate, s_infrastruture.EndDate));
            CreateMap<IEnumerable<Entities.Server>, IEnumerable<Core.Domain.Entities.Server>>();
            //CreateMap<IEnumerable<Core.Domain.Entities.Server>, IEnumerable<Entities.Server>>();
        }
    }
}

// == null?Guid.Empty:s.DeletedBy