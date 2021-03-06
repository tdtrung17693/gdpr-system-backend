﻿using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Dto.GatewayResponses.Repositories;
using Web.Api.Core.Dto.UseCaseRequests.CustomerUseCaseRequest;
using Web.Api.Core.Dto.UseCaseResponses;

namespace Web.Api.Core.Interfaces.UseCases.ICustomerUseCases
{
    public interface IExportCustomerUseCase : IUseCaseRequestHandler<ExportCustomerRequest, ExportCustomerResponse>
    {
    }
}
