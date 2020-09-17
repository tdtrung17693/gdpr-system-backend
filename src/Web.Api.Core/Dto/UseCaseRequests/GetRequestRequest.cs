using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class GetRequestRequest : IUseCaseRequest<GetRequestResponse>
    {
        public GetRequestRequest(Guid uid,int pageNo, int pageSize, string keyword, string filterStatus, /*DateTime? fromDateExport , DateTime? toDateExport, */string type)
        {
            FilterStatus = filterStatus;
            PageNo = pageNo;
            PageSize = pageSize;
            Keyword = keyword;
            /*FromDateExport = fromDateExport;
            ToDateExport = toDateExport;*/
            Type = type;
            Uid = uid;
        }

        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string Keyword { get; set; }
        public string FilterStatus { get; set;}
        public string Type { get; set; }
        public DateTime? FromDateExport { get; set; }
        public DateTime? ToDateExport { get; set; }
        public Guid Uid { get; set; }
    }
}