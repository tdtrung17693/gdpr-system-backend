﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.Services;
using Web.Api.Core.Interfaces.UseCases;

namespace Web.Api.Core.UseCases.Request
{
    public class GetEachRequestUseCase : IGetEachRequestUseCase
    {
        private readonly IRequestRepository _requestRepository;
        private Domain.Entities.User _currentUser;

        public GetEachRequestUseCase(IRequestRepository requestRepository, IAuthService authService)
        {
            _requestRepository = requestRepository;
            _currentUser = authService.GetCurrentUser();
        }

        public async Task<bool> Handle(GetEachRequestRequest message, IOutputPort<GetEachRequestResponse> outputPort)
        {
            var request = _requestRepository.getEachRequest(message.RequestId, _currentUser.Role.Name);
            outputPort.Handle(new GetEachRequestResponse(request, true));
            return true;
        }
    }
}
