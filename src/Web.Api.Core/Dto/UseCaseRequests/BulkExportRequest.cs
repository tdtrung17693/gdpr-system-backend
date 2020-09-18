using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class BulkExportRequest : IUseCaseRequest<BulkExportResponse>
    {
        public BulkExportRequest(string idList)
        {
            IdList = idList;
        }

        public string IdList { get; set; }
    }
}
