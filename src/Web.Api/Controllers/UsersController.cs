using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Interfaces.Gateways.Repositories;

using Web.Api.Infrastructure.Helpers;
using Web.Api.Core.Dto.UseCaseRequests.User;
using Web.Api.Core.UseCases.User;
using Web.Api.Presenters;
using Web.Api.Core.Dto.UseCaseResponses.User;
using Web.Api.Serialization;
using Web.Api.Models.Request;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController : ControllerBase
  {
    //private IUserRepository _userRepository;
    private ManageUserUseCase _userUseCase;
    private ResourcePresenter<ReadUserResponse> _readUserPresenter;
    private ResourcePresenter<CreateUserResponse> _createUserPresenter;
    public UsersController(
      ManageUserUseCase userUseCase,
      ResourcePresenter<ReadUserResponse> readUserPresenter,
      ResourcePresenter<CreateUserResponse> createUserPresenter)
    {
      _userUseCase = userUseCase;
      _readUserPresenter = readUserPresenter;
      _createUserPresenter = createUserPresenter;
    }
    // GET: api/<UsersController>
    [Authorize("CanViewUser")]
    [HttpGet]
    public async Task<ActionResult> Get([FromQuery] PagedRequest request)
    {
      _readUserPresenter.HandleResource = r =>
      {
        return r.Success ? JsonSerializer.SerializeObject(r.Users) : JsonSerializer.SerializeObject(r.Errors);
      };
      await _userUseCase.Read(
        new ReadUserRequest(request.Page, request.FilterBy, request.SortedBy, request.SortOrder),
        _readUserPresenter);
      return _readUserPresenter.ContentResult;
    }

    // GET api/<UsersController>/5
    [HttpGet("{id}")]
    [Authorize("CanViewUser")]
    public async Task<ActionResult> Get(string id)
    {
      _readUserPresenter.HandleResource = r =>
      {
        return r.Success ? JsonSerializer.SerializeObject(r.User) : JsonSerializer.SerializeObject(r.Errors);
      };

      await _userUseCase.Read(new ReadUserRequest(id), _readUserPresenter);
      return _readUserPresenter.ContentResult;
    }

    // POST api/<UsersController>
    [HttpPost]
    [Authorize("CanCreateUser")]
    public async Task<ActionResult> Post(CreateUserRequest request)
    {
      if (!ModelState.IsValid)
      { // re-render the view when validation failed.
        return BadRequest(ModelState);
      }
      //await _registerUserUseCase.Handle(new RegisterUserRequest(request.FirstName,request.LastName,request.Email, request.UserName,request.Password), _registerUserPresenter);
      return _createUserPresenter.ContentResult;
    }

    // PUT api/<UsersController>/5
    [HttpPut("{id}")]
    [Authorize("CanEditUser")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<UsersController>/5
    [HttpDelete("{id}")]
    [Authorize("CanDeleteUser")]
    public void Delete(int id)
    {
    }
  }
}
