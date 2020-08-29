using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Domain.Entities;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface IServerRepository
    {
        IEnumerable<Server> GetAllCommand();
    }
}
