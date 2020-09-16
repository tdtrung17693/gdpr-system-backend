using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Core.Dto.UseCaseResponses
{
    public class ManageRequestResponse : UseCaseResponseMessage
    {
        public ManageRequestResponse(bool success = true, string message = null) : base(success, message)
        {
        }
    }
}
