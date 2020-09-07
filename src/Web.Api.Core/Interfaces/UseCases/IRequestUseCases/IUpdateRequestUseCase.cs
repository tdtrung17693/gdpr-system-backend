using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses.RequestUseCaseResponse;

namespace Web.Api.Core.Interfaces.UseCases
{
    public interface IUpdateRequestUseCase : IUseCaseRequestHandler<UpdateRequestRequest, UpdateRequestResponse>
    {
    }
}
