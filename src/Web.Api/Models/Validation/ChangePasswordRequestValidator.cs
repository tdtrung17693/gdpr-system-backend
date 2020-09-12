using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Models.Request;

namespace Web.Api.Models.Validation
{
  public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
  {
    public ChangePasswordRequestValidator()
    {
      RuleFor(r => r.CurrentPassword).NotEmpty();
      RuleFor(r => r.NewPassword).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().MinimumLength(8);
      RuleFor(r => new { r.CurrentPassword, r.NewPassword }).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(r =>
      {
        return r.CurrentPassword != r.NewPassword;
      }).WithMessage("Nothing to change.");
      RuleFor(r => r.ConfirmPassword).NotEmpty();
      RuleFor(r => new { r.NewPassword, r.ConfirmPassword }).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(r =>
      {
        return r.ConfirmPassword == r.NewPassword;
      }).WithMessage("Password not match.");
    }
  }
}
