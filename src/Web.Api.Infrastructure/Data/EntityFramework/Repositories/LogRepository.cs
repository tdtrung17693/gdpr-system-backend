using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Domain.Event;
using Web.Api.Core.Dto;
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

        public LogRepository(ApplicationDbContext context, IAuthService authService, IDomainEventBus eventBus,
            IMapper mapper)
        {
            _context = context;
            _currentUser = authService.GetCurrentUser();
            _eventBus = eventBus;
            _mapper = mapper;
        }

        public async Task<DataTable> Create(CreateLog logMessage, User creator)
        {
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


        public async Task<DataTable> LogNewRequest(Guid _requestId, User creator)
        {
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                var requestId = new SqlParameter("@RequestId ", _requestId);
                var updatedField = new SqlParameter("@UpdatedField", "RequestStatus");
                var updatedState = new SqlParameter("@UpdatedState", "New");
                var previousState = new SqlParameter("@PreviousState", "");
                var message = new SqlParameter("@Message", (creator.FirstName + ' ' + creator.LastName));
                var createBy = new SqlParameter("@CreatedBy", creator.Id);
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
                
                if (command.Connection.State == ConnectionState.Closed)
                    await command.Connection.OpenAsync();
                
                try
                {
                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                }

                return dt;
            }
        }

        public async Task LogUpdateRequest(Guid requestId, Dictionary<string, List<string>> updatedFields,
            User updator)
        {
            var newLogTable = new DataTable();
            newLogTable.Columns.Add("UpdatedField", typeof(string));
            newLogTable.Columns.Add("PreviousState", typeof(string));
            newLogTable.Columns.Add("UpdatedState", typeof(string));
            newLogTable.Columns.Add("Message", typeof(string));

            foreach (var updatedField in updatedFields)
            {
                var newRow = newLogTable.NewRow();
                newRow["UpdatedField"] = updatedField.Key;
                newRow["PreviousState"] = updatedField.Value.First();
                newRow["UpdatedState"] = updatedField.Value.Skip(1).First();
                newRow["Message"] = updator.FirstName + ' ' + updator.LastName;
                ;
                newLogTable.Rows.Add(newRow);
            }

            var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "CreateMultipleLogs";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@NewLogs", newLogTable));
            command.Parameters.Add(new SqlParameter("@RequestId", Convert.ToString(requestId)));
            command.Parameters.Add(new SqlParameter("@CreatedBy", Convert.ToString(updator.Id)));

            if (command.Connection.State == ConnectionState.Closed)
                await command.Connection.OpenAsync();
            try
            {
                var reader = await command.ExecuteReaderAsync();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        public async Task LogAcceptRejectRequest(Guid requestId, User updator, string newRequestStatus)
        {
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "CreateHistoryLog";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RequestId ", requestId));
                command.Parameters.Add(new SqlParameter("@UpdatedField", "RequestStatus"));
                command.Parameters.Add(new SqlParameter("@UpdatedState", newRequestStatus));
                command.Parameters.Add(new SqlParameter("@PreviousState", "New"));
                command.Parameters.Add(new SqlParameter("@Message", updator.FirstName + ' ' + updator.LastName));
                command.Parameters.Add(new SqlParameter("@CreatedBy", updator.Id));
                DataTable dt = new DataTable();
                _context.Database.OpenConnection();
                dt.Load(command.ExecuteReader());
            }
        }

        public async Task<DataTable> Log(CreateLog logMessage, User creator)
        {
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

        public async Task<IEnumerable<HistoryLogDto>> GetListLogOfRequest(Guid requestId)
        {
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                var _requestId = new SqlParameter("@requestId", requestId);

                // var commandText = "exec getListServerFilter @filterKey";
                command.CommandText = "GetHistoryLog";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(_requestId);
                
                _context.Database.OpenConnection();
                
                var result = _mapper.Map<IEnumerable<HistoryLogDto>>(command.ExecuteReader());
                return result;
            }
        }
    }
}
