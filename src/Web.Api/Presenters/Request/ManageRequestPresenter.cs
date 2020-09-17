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
    public class ManageRequestPresenter : IOutputPort<ManageRequestResponse>
    {
        public JsonContentResult ContentResult { get; }

        public ManageRequestPresenter()
        {
            ContentResult = new JsonContentResult();
        }

        public void Handle(ManageRequestResponse response)
        {
            ContentResult.StatusCode = (int)(response.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
        }
    }
}
