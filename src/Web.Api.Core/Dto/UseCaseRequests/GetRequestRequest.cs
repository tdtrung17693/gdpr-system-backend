using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class GetRequestRequest : IUseCaseRequest<GetRequestResponse>
    {
        public GetRequestRequest(int pageNo, int pageSize, string type)
        {
            PageNo = pageNo;
            PageSize = pageSize;
            Type = type;
        }

        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string Type { get; set; }
    }
}