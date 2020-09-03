using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Interfaces.UseCases;
using Web.Api.Presenters;

namespace Web.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AccountsController : ControllerBase
  {
    private readonly RegisterUserPresenter _registerUserPresenter;

    public AccountsController(RegisterUserPresenter registerUserPresenter)
    {
      _registerUserPresenter = registerUserPresenter;
    }

    // POST api/accounts
    [HttpPost]
    public async Task<ActionResult> Post()
    {
      if (!ModelState.IsValid)
      { // re-render the view when validation failed.
        return BadRequest(ModelState);
      }
      //await _registerUserUseCase.Handle(new RegisterUserRequest(request.FirstName,request.LastName,request.Email, request.UserName,request.Password), _registerUserPresenter);
      return _registerUserPresenter.ContentResult;
    }

    [HttpGet]
    public string Get()
    {
      return "Hey there";
    }
  }
}
