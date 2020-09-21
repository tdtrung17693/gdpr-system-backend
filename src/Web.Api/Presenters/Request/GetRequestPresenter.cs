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
    public sealed class GetRequestPresenter : IOutputPort<GetRequestResponse>
    {
        public JsonContentResult ContentResult { get; }

        public GetRequestPresenter()
        {
            ContentResult = new JsonContentResult();
        }

        public void Handle(GetRequestResponse response)
        {
            ContentResult.Content = JsonConvert.SerializeObject( response.RequestList);
            ContentResult.StatusCode = (int)(response.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);

        }
    }
}