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
    public sealed class UpdateServerPresenter : IOutputPort<UpdateServerResponse>
    {
        public JsonContentResult ContentResult { get; }

        public UpdateServerPresenter()
        {
            ContentResult = new JsonContentResult();
        }

        public void Handle(UpdateServerResponse response)
        {
            ContentResult.StatusCode = (int)(response.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
        }
    }
}
