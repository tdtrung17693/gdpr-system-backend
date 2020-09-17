using System;
using System.Collections.Generic;
using Web.Api.Core.Interfaces;
using DomainEntities = Web.Api.Core.Domain.Entities;

namespace Web.Api.Core.Dto.UseCaseResponses
{
    public class ReadServerResponse : UseCaseResponseMessage
    {
        public Pagination<DomainEntities.Server> Servers { get; }
        public DomainEntities.Server Server { get; }
        public IEnumerable<Error> Errors { get; }

        public ReadServerResponse(IEnumerable<Error> errors, bool success = false, string message = null) : base(success, message)
        {
            Errors = errors;
        }

        public ReadServerResponse(Pagination<DomainEntities.Server> servers, bool success = true, string message = null) : base(success, message)
        {
            Servers = servers;
        }
        public ReadServerResponse(DomainEntities.Server server, bool success = true, string message = null) : base(success, message)
        {
            Server = server;
        }
    }
}
