using System;
using System.Collections.Generic;
using System.Text;

using Web.Api.Core.Interfaces;
using Web.Api.Core.Dto.UseCaseResponses;
using System.Data;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class BulkRequestRequest: IUseCaseRequest<BulkRequestResponse>
    {
        public DataTable IdList { get; set; }
        public string Status { get; set; }
        public Guid Updator { get; set; }


        public BulkRequestRequest(DataTable idList, string status, Guid updator)
        {
            IdList = idList;
            Status = status;
            Updator = updator;
        }
    }
}
