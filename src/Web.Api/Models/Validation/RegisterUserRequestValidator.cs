using FluentValidation;
using Web.Api.Models.Request;

namespace Web.Api.Models.Validation
{
    public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserRequestValidator()
        {
            RuleFor(x => x.FirstName).Length(2, 30).NotEmpty();
            RuleFor(x => x.LastName).Length(2, 30).NotEmpty();
            RuleFor(x => x.UserName).Length(5, 255).NotEmpty();
            RuleFor(x => x.Password).Length(6, 15).NotEmpty();
        }
    }
}
