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
        public GetRequestRequest(int pageNo, int pageSize, string keyword, string filterStatus, string type)
        {
            FilterStatus = filterStatus;
            PageNo = pageNo;
            PageSize = pageSize;
            Keyword = keyword;
            Type = type;
        }

        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string Keyword { get; set; }
        public string FilterStatus { get; set;}
        public string Type { get; set; }
    }
}