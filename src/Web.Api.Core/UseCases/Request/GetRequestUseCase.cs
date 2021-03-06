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
using Web.Api.Core.Domain.Entities;

namespace Web.Api.Core.UseCases.Request
{
    public class GetRequestUseCase : IGetRequestUseCase
    {
        private readonly IRequestRepository _requestRepository;
        private Domain.Entities.User _currentUser;
        public GetRequestUseCase(IRequestRepository requestRepository, IAuthService authService)
        {
            _requestRepository = requestRepository;
            _currentUser = authService.GetCurrentUser();
        }

        public async Task<bool> Handle(GetRequestRequest message, IOutputPort<GetRequestResponse> outputPort)
        {
                var listRequests = await _requestRepository.GetRequest(_currentUser.Id, message.PageNo, message.PageSize, message.Keyword, message.FilterStatus);
                //var noPages = await _requestRepository.getNoPages(message.PageSize);
                outputPort.Handle(new GetRequestResponse(10, listRequests, true));
                return true;
        }
    }
}
