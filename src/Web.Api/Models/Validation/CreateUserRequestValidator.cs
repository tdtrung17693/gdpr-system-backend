using System;
using FluentValidation;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Models.Request;

namespace Web.Api.Models.Validation
{
  public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
  {
    public CreateUserRequestValidator(IUserRepository userRepository, IRoleRepository roleRepository)
    {
      RuleFor(x => x.FirstName).NotEmpty().Length(2, 30);
      RuleFor(x => x.LastName).NotEmpty().Length(2, 30);

      RuleFor(x => x.Username).Cascade(CascadeMode.StopOnFirstFailure)
        .NotEmpty()
        .Length(5, 255)
        .MustAsync(async (userName, cancellation) =>
      {
        var user = await userRepository.FindByName(userName);
        return user == null;
      }).WithMessage("Username existed");

      RuleFor(x => x.Email).Cascade(CascadeMode.StopOnFirstFailure)
        .NotEmpty()
        .EmailAddress()
        .MustAsync(async (email, cancellation) =>
        {
          var user = await userRepository.FindByEmail(email);
          return user == null;
        }).WithMessage("Email existed");

      RuleFor(x => x.RoleId).Cascade(CascadeMode.StopOnFirstFailure)
        .NotEmpty()
        .MustAsync(async (id, cancellation) =>
        {
          try
          {
            var guid = Guid.Parse(id);
            return await roleRepository.IsExisted(guid);
          }
          catch (FormatException e)
          {
            return false;
          }
        }).WithMessage("Invalid role");
    }
  }
}
