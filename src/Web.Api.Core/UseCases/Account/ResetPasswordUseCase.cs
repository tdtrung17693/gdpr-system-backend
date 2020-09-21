using System.Threading.Tasks;
using Web.Api.Core.Dto.UseCaseRequests.Account;
using Web.Api.Core.Dto.UseCaseResponses.Account;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases.Account;

namespace Web.Api.Core.UseCases.Account
{
    public class ResetPasswordUseCase : IResetPasswordUseCase
    {
        private readonly IUserRepository _userRepository;
        public ResetPasswordUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(ResetPasswordRequest message, IOutputPort<ResetPasswordResponse> outputPort)
        {
            var response = await _userRepository.ResetPassword(message.UserEmail);
            if (response.Success)
            {
                outputPort.Handle(new ResetPasswordResponse());
                return true;
            }
            
            outputPort.Handle(new ResetPasswordResponse(response.Errors));
            return false;
        }
    }
}