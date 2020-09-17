using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Dto.UseCaseResponses.ServerUseCaseResponse;
using Web.Api.Core.Interfaces;

namespace Web.Api.Presenters
{
    public sealed class BulkServerPresenter : IOutputPort<BulkServerResponse>
    {
        public JsonContentResult ContentResult { get; }

        public BulkServerPresenter()
        {
            ContentResult = new JsonContentResult();
        }

        public void Handle(BulkServerResponse response)
        {
            ContentResult.StatusCode = (int)(response.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
        }
    }
}
