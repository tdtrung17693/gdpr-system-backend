using AutoMapper;
using Web.Api.Core.Domain.Entities;
using Web.Api.Infrastructure.Data;

namespace Web.Api.Infrastructure.Data.Mapping
{
    public class RequestProfile : Profile
    {
        public RequestProfile()
        {
            CreateMap<SPRequestResultView, RequestDetail>().ConstructUsing(res => new RequestDetail(
                res.Title,
                res.StartDate,
                res.EndDate,
                res.ServerId,
                res.Description,
                res.RequestStatus,
                res.Response,
                res.ApprovedBy,
                res.Id,
                res.CreatedBy,
                res.CreatedAt,
                res.UpdatedBy,
                res.UpdatedAt,
                null,
                null,
                res.ServerName ,
                res.ServerIP,
                (res.CreatedByFName != null && res.CreatedByLName != null) ? res.CreatedAt +"   -   " + res.CreatedByFName + " " + res.CreatedByLName + " - " + res.CreatedByEmail: "God",
                (res.UpdatedByFName != null && res.UpdatedByLName != null) ? res.UpdatedAt + "   -   " + res.UpdatedByFName + " " + res.UpdatedByLName + " - " + res.UpdatedByEmail : "-"
                ));
        }
    }
}
