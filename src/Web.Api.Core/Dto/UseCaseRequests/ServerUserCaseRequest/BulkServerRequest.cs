﻿using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Dto.UseCaseResponses.ServerUseCaseResponse;
using System.Data;

namespace Web.Api.Core.Dto.UseCaseRequests.ServerUserCaseRequest
{
    public class BulkServerRequest: IUseCaseRequest<BulkServerResponse>
    {
        public DataTable IdList { get; set; }
        public bool Status { get; set; }
        public Guid Updator { get; set; }
        

        public BulkServerRequest(DataTable idList, bool status, Guid updator)
        {
            IdList = idList;
            Status = status;
            Updator = updator;
        }
    }

}
