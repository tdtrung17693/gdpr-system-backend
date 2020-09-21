using System.Collections.Generic;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseResponses.Account
{
    public class ResetPasswordResponse : UseCaseResponseMessage 
    {
        public IEnumerable<Error> Errors {  get; }

        public ResetPasswordResponse(IEnumerable<Error> errors, bool success=false, string message=null) : base(success, message)
        {
            Errors = errors;
        }

        public ResetPasswordResponse(bool success = true, string message = null) : base(success, message)
        {}
    }
}