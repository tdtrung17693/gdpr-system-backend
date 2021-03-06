﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Presenters
{
    public sealed class CreateRequestPresenter : IOutputPort<CreateRequestResponse>
    {
        public JsonContentResult ContentResult { get; }

        public CreateRequestPresenter()
        {
            ContentResult = new JsonContentResult();
        }

        public void Handle(CreateRequestResponse response)
        {
            ContentResult.StatusCode = (int)(response.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
        }
    }
}