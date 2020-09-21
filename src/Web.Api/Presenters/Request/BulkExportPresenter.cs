using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Presenters.Request
{
    public class BulkExportPresenter : IOutputPort<BulkExportResponse>
    {
        public ContentResult ContentResult { get; }
        public BulkExportPresenter()
        {
            ContentResult = new ContentResult();
            ContentResult.ContentType = "application/json";
        }

        public void Handle(BulkExportResponse response)
        {
            ContentResult.StatusCode = (int)(response.Success ? HttpStatusCode.OK : HttpStatusCode.BadGateway);
            ContentResult.Content = response.Success ? JsonConvert.SerializeObject(response.ResponsedRequest) : JsonConvert.SerializeObject(response.Errors);
        }
    }
}
