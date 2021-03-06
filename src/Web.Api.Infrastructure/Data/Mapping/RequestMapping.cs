﻿using AutoMapper;
using System;
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
                String.Format("{0:F}", res.StartDate),
                String.Format("{0:F}", res.EndDate),
                res.ServerId,
                res.Description,
                res.RequestStatus,
                res.Response,
                res.ApprovedBy,
                res.Id,
                res.CreatedBy,
                String.Format("{0:F}", res.CreatedAt),
                res.UpdatedBy,
                String.Format("{0:F}", res.UpdatedAt),
                null,
                null,
                res.ServerName,
                res.ServerIP,
                (res.CreatedByFName != null && res.CreatedByLName != null) ? res.CreatedByFName + " " + res.CreatedByLName : "-",
                res.CreatedByEmail,
                (res.CreatedByFName != null && res.CreatedByLName != null) ? res.CreatedByFName + " " + res.CreatedByLName + " - " + res.CreatedByEmail : "God",
                (res.UpdatedByFName != null && res.UpdatedByLName != null) ? res.UpdatedByFName + " " + res.UpdatedByLName : "-",
                res.UpdatedByEmail,
                (res.UpdatedByFName != null && res.UpdatedByLName != null) ? res.UpdatedByFName + " " + res.UpdatedByLName + " - " + res.UpdatedByEmail : "-",
                res.RoleName
                ));

            CreateMap<SPRequestResultExportView, ExportRequestDetail>().ConstructUsing(res => new ExportRequestDetail(
                res.Title,
                String.Format("{0:F}", res.StartDate) ,
                String.Format("{0:F}", res.EndDate),
                res.Description,
                res.RequestStatus,
                res.Response,
                String.Format("{0:F}", res.CreatedAt),
                String.Format("{0:F}", res.UpdatedAt),
                res.ServerName,
                res.ServerIP,
                res.ApprovedUserEmail,
                res.CreatedUserEmail,
                res.UpdatedUserEmail
                ));
        }
    }
}
