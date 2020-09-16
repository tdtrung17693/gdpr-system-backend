using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;
using Web.Api.Serialization;

namespace Web.Api.Presenters
{
    public class ExportPresenter : IOutputPort<ExportResponse>
    {
        public JsonContentResult ContentResult { get; }

        public ExportPresenter()
        {
            ContentResult = new JsonContentResult();
        }

        public void Handle(ExportResponse response)
        {
            ContentResult.StatusCode = (int)(response.Success ? HttpStatusCode.OK : HttpStatusCode.Unauthorized);
            ContentResult.Content = JsonConvert.SerializeObject(response.ResponsedRequest);
        }
    }
}
