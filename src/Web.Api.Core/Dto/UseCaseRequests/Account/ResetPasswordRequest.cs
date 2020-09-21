using Web.Api.Core.Dto.UseCaseResponses.Account;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests.Account
{
    public class ResetPasswordRequest : IUseCaseRequest<ResetPasswordResponse>
    {
        public string UserEmail { get; }

        public ResetPasswordRequest(string userEmail)
        {
            UserEmail = userEmail;
        }
    }
}