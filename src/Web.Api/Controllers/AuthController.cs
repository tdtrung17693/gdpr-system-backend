using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases;
using Web.Api.Presenters;

namespace Web.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly ILoginUseCase _loginUseCase;
    private readonly LoginPresenter _loginPresenter;
    private readonly IRoleRepository _roleRepository;

    public AuthController(ILoginUseCase loginUseCase, LoginPresenter loginPresenter, IRoleRepository roleRepository)
    {
      _loginUseCase = loginUseCase;
      _loginPresenter = loginPresenter;
      _roleRepository = roleRepository;
    }

    // POST api/auth/login
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] Models.Request.LoginRequest request)
    {
      if (!ModelState.IsValid)
      { // re-render the view when validation failed.
        return BadRequest(ModelState);
      }
      await _loginUseCase.Handle(new LoginRequest(request.UserName, request.Password), _loginPresenter);
      return _loginPresenter.ContentResult;
    }

    [HttpGet("Role/GetAll")]
    public async Task<IEnumerable<object>> GetAllRoles()
    {
      return (await _roleRepository.FindAll()).Select(r => new { r.Name, r.Id });
    }
  }
}
