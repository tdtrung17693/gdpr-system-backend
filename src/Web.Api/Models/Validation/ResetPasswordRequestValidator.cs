using FluentValidation;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Models.Request;

namespace Web.Api.Models.Validation
{
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator(IUserRepository userRepository)
        {
            RuleFor(r => r.Email).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .EmailAddress()
                .MustAsync(async (email, cancellation) => (await userRepository.FindByEmail(email)) != null)
                .WithMessage("Email address doesn't exist.");
        }
    }
}