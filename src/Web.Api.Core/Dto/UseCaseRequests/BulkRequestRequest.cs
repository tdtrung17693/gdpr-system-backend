using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class BulkRequestRequest
    {
        public DataTable IdList { get; set; }
        public bool Status { get; set; }
        public Guid Updator { get; set; }


        public BulkRequestRequest(DataTable idList, bool status, Guid updator)
        {
            IdList = idList;
            Status = status;
            Updator = updator;
        }
    }
}
