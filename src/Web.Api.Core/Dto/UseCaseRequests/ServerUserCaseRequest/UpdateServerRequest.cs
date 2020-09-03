using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Dto.UseCaseResponses.ServerUseCaseResponse;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests.ServerUserCaseRequest
{
    public class UpdateServerRequest: IUseCaseRequest<UpdateServerResponse>
    {
        public string Name { get; set; }

        public string IpAddress { get; set; }
        public UpdateServerRequest(string name, string ipAddress)
        {
            Name = name;
            IpAddress = ipAddress;
        }
    }
   
}
