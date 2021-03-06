﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseRequests.Account;
using Web.Api.Core.Dto.UseCaseResponses.Account;
using Web.Api.Core.Dto.UseCaseResponses.User;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.Services;
using Web.Api.Core.Interfaces.UseCases;
using Web.Api.Core.Interfaces.UseCases.Account;
using Web.Api.Presenters;
using Web.Api.Serialization;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorylogController : ControllerBase
    {
        private readonly ILogRepository _logRepository;
        private readonly IMapper _mapper;

        public HistorylogController(
             ILogRepository logRepository,
            IMapper mapper
          )
        {
            _mapper = mapper;
            _logRepository = logRepository;
        }

        [HttpGet("{requestId}")]
        [Authorize("CanEditRequest")]
        public async Task<IEnumerable<HistoryLogDto>> GetHistoryLog(Guid requestId)
        {
            var result = await _logRepository.GetListLogOfRequest(requestId);
            foreach (var r in result)
            {
              r.CreatedAt = r.CreatedAt.ToLocalTime();
            }
            
            return result;
        }

    }
}
