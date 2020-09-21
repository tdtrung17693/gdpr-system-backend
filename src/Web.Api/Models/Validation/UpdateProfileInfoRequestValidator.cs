using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Models.Request;

namespace Web.Api.Models.Validation
{
  public class UpdateProfileInfoRequestValidator : AbstractValidator<UpdateProfileInfoRequest>
  {
    public UpdateProfileInfoRequestValidator()
    {
      RuleFor(x => x.FirstName).NotEmpty();
      RuleFor(x => x.LastName).NotEmpty();
    }
  }
}
