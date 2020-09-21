using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Dto.UseCaseResponses.RequestUseCaseResponse;
using Web.Api.Core.Interfaces;

namespace Web.Api.Presenters
{
    public sealed class UpdateRequestPresenter : IOutputPort<UpdateRequestResponse>
    {
        public JsonContentResult ContentResult { get; }

        public UpdateRequestPresenter()
        {
            ContentResult = new JsonContentResult();
        }

        public void Handle(UpdateRequestResponse response)
        {
            ContentResult.StatusCode = (int)(response.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
        }
    }
}
