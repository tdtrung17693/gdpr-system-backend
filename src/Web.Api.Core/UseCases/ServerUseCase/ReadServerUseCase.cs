using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases;
using DomainEntities = Web.Api.Core.Domain.Entities;

namespace Web.Api.Core.UseCases
{
    public sealed class ReadServerUseCase : IReadServerUseCase
    {
        private IServerRepository _serverRepository;
        public ReadServerUseCase(IServerRepository serverRepository)
        {
            _serverRepository = serverRepository;
        }

        public async Task<bool> Handle(ReadServerRequest message, IOutputPort<ReadServerResponse> outputPort)
        {
            if (message.ServerId.ToString() != Guid.Empty.ToString())
            {
                DomainEntities.Server server = await _serverRepository.FindById(message.ServerId);
                if (server == null)
                {
                    outputPort.Handle(new ReadServerResponse(new[] { new Error(Error.Codes.ENTITY_NOT_FOUND, Error.Messages.ENTITY_NOT_FOUND) }));
                }
                else
                {
                    outputPort.Handle(new ReadServerResponse(server));
                }
            }
            else
            {
                IPagedCollection<DomainEntities.Server> servers = _serverRepository.FindAll();
                var filterStr = message.FilterString;
                if (filterStr.Contains(",") && filterStr.Contains(":") || filterStr.Contains(":"))
                {
                    var filterCriteria = filterStr.Split(",");
                    foreach (var crit in filterCriteria)
                    {
                        var keyVal = crit.Split(":");

                        servers.FilterBy(keyVal[0], keyVal[1]);
                    }
                }
                else
                {
                    servers.FilterBy(s => s.Name.Contains(filterStr) || s.IpAddress.Contains(filterStr));
                }
                if (message.SortedBy == "Name")
                {
                    servers.SortBy(s => s.Name, message.SortOrder);
                }
                else
                {
                    servers.SortBy(message.SortedBy, message.SortOrder);
                }
                var items = await servers.GetItemsForPage(message.Page, message.PageSize);
                var pagination = new Pagination<DomainEntities.Server>
                {
                    Items = items,
                    TotalItems = servers.TotalItems(),
                    TotalPages = servers.TotalPages(),
                    Page = message.Page
                };

                outputPort.Handle(new ReadServerResponse(pagination));
            }

            return true;
        }
    }
}
