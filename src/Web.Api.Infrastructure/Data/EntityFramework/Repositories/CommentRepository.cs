using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Domain.Event;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.GatewayResponses.Repositories;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.Services.Event;

namespace Web.Api.Infrastructure.Data.EntityFramework.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IDomainEventBus _eventBus;

        public CommentRepository(ApplicationDbContext context, IDomainEventBus eventBus)
        {
            _context = context;
            _eventBus = eventBus;
        }

        public async Task<IEnumerable<Comment>> FindCommentsOfRequest(Guid requestId)
        {
            var query = _context.Comment.Include(c => c.Author).Where(c => c.RequestId == requestId);
            query = query.OrderBy(c => c.CreatedAt);
            return await query.ToListAsync();
        }

        public async Task<CreateCommentResponse> CreateCommentOfRequest(Guid requestId, string content, User author,
            Guid? parentId)
        {
            var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "CreateComment";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@Content", content));
            command.Parameters.Add(new SqlParameter("@RequestId", requestId));
            command.Parameters.Add(new SqlParameter("@ParentId", parentId));
            command.Parameters.Add(new SqlParameter("@AuthorId", author.Id));

            if (command.Connection.State == ConnectionState.Closed)
                await command.Connection.OpenAsync();
            
            try
            {
                var reader = await command.ExecuteReaderAsync();
                await reader.ReadAsync();
                var newId = Guid.Parse(reader["Id"].ToString());
                var createdAt = Convert.ToDateTime(reader["CreatedAt"]);
                await _eventBus.Trigger(new CommentCreated(
                    newId,
                    requestId,
                    content,
                    author.FirstName,
                    author.LastName,
                    createdAt,
                    parentId));
                return new CreateCommentResponse(newId, createdAt);
            }
            catch (SqlException e)
            {
                // Unique constraint violation code number
                return new CreateCommentResponse(new[]
                {
                    new Error(Error.Codes.UNKNOWN, Error.Messages.UNKNOWN)
                });
            }
            finally
            {
                command.Connection.Close();
            }
        }
    }
}