using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Presenters
{
    public sealed class GetEachRequestPresenter : IOutputPort<GetEachRequestResponse>
    {
        public JsonContentResult ContentResult { get; }

        public GetEachRequestPresenter()
        {
            ContentResult = new JsonContentResult();
        }

        public void Handle(GetEachRequestResponse response)
        {
            ContentResult.StatusCode = (int)(response.Success ? HttpStatusCode.OK : HttpStatusCode.NotAcceptable);
            ContentResult.Content = response.Success ? JsonConvert.SerializeObject(response) : JsonConvert.SerializeObject(response.Errors);
        }
    }
}