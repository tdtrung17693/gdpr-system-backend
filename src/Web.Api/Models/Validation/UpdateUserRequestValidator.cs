using FluentValidation;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Models.Request;

namespace Web.Api.Models.Validation
{
  public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
  {
    public UpdateUserRequestValidator(IRoleRepository roleRepository)
    {
      RuleFor(x => x.RoleId).Cascade(CascadeMode.StopOnFirstFailure)
        .MustAsync(async (id, cancellation) => await roleRepository.IsExisted(id)).WithMessage("Invalid role");
    }
  }
}
