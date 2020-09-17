using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseResponses
{
    public class ManageServerResponse: UseCaseResponseMessage
    {
        public IEnumerable<string> Errors { get; }

        public ManageServerResponse(IEnumerable<string> errors, bool success = false, string message = null)
            : base(success, message)
        {
            Errors = errors;
        }

        public ManageServerResponse(bool success = false, string message = null)
            : base(success, message)
        {
        }
    }
}
