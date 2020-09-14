using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses.User;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.Services;
using Web.Api.Core.Interfaces.UseCases;
using Web.Api.Presenters;

namespace Web.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AccountsController : ControllerBase
  {
    private readonly RegisterUserPresenter _registerUserPresenter;
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    private readonly INotificationRepository _notiRepo;

    public AccountsController(RegisterUserPresenter registerUserPresenter, IAuthService authService, IMapper mapper, INotificationRepository notiRepo)
    {
      _registerUserPresenter = registerUserPresenter;
      _authService = authService;
      _mapper = mapper;
      _notiRepo = notiRepo;
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

    [HttpGet("me")]
    [Authorize()]
    public async Task<ActionResult<object>> CurrentUser()
    {
      var user = _authService.GetCurrentUser();
      var notifications = await _notiRepo.GetNotificationOf((System.Guid)user.Id);

      return new
      {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Username = user.Account.Username,
        Role = user.Role.Name,
        RoleId = user.RoleId,
        Permissions = _authService.GetAllPermissions(),
        Notifications = notifications
      };
    }
  } 
}
