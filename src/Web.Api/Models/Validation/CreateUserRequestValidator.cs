using FluentValidation;
using Web.Api.Core.Dto.UseCaseRequests.User;
using Web.Api.Models.Request;

namespace Web.Api.Models.Validation
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(x => x.FirstName).Length(2, 30).NotEmpty();
            RuleFor(x => x.LastName).Length(2, 30).NotEmpty();
            RuleFor(x => x.Username).Length(5, 255).NotEmpty();
            RuleFor(x => x.Password).Length(6, 15).NotEmpty();
        }
    }
}
