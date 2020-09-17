using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Domain.Event;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.GatewayResponses.Repositories;
using Web.Api.Core.Dto.Requests;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.Services;
using Web.Api.Core.Interfaces.Services.Event;

namespace Web.Api.Infrastructure.Data.EntityFramework.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly User _currentUser;
        private readonly IDomainEventBus _eventBus;
        private readonly IMapper _mapper;
        public LogRepository(ApplicationDbContext context, IAuthService authService, IDomainEventBus eventBus, IMapper mapper)
        {
            _context = context;
            _currentUser = authService.GetCurrentUser();
            _eventBus = eventBus;
            _mapper = mapper;
        }
        public async Task<DataTable> Create(CreateLog logMessage, User creator) {
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                var requestId = new SqlParameter("@RequestId ", logMessage.RequestId);
                var updatedField = new SqlParameter("@UpdatedField", logMessage.UpdatedField);
                var updatedState = new SqlParameter("@UpdatedState", logMessage.UpdatedState);
                var previousState = new SqlParameter("@PreviousState", logMessage.PreviousState);
                var message = new SqlParameter("@Message", (creator.FirstName + ' ' + creator.LastName));
                var createBy = new SqlParameter("@CreatedBy", logMessage.CreatedBy);
                // var commandText = "exec getListServerFilter @filterKey";
                command.CommandText = "CreateHistoryLog";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(requestId);
                command.Parameters.Add(updatedField);
                command.Parameters.Add(updatedState);
                command.Parameters.Add(previousState);
                command.Parameters.Add(message);
                command.Parameters.Add(createBy);
                DataTable dt = new DataTable();
                _context.Database.OpenConnection();
                dt.Load(command.ExecuteReader());
                return dt;
            }
        }
    }
}
