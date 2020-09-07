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
using AutoMapper;
using Web.Api.Core.Dto;

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
    private IMapper _mapper;
    public UsersController(
      ManageUserUseCase userUseCase,
      ResourcePresenter<ReadUserResponse> readUserPresenter,
      ResourcePresenter<CreateUserResponse> createUserPresenter,
      IMapper mapper)
    {
      _userUseCase = userUseCase;
      _readUserPresenter = readUserPresenter;
      _createUserPresenter = createUserPresenter;
      _mapper = mapper;
    }
    // GET: api/<UsersController>
    [Authorize("CanViewUser")]
    [HttpGet]
    public async Task<ActionResult> Get([FromQuery] PagedRequest request)
    {
      _readUserPresenter.HandleResource = r =>
      {
        var users = _mapper.Map<Pagination<User>, Pagination<UserDTO>>(r.Users);

        return r.Success ? JsonSerializer.SerializeObject(users) : JsonSerializer.SerializeObject(r.Errors);
      };

      var filterString = request.FilterBy == null ? "" : request.FilterBy;
      await _userUseCase.Read(
        new ReadUserRequest(request.Page, request.PageSize, filterString, request.SortedBy, request.SortOrder),
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
        var user = _mapper.Map<User, UserDTO>(r.User);
        return r.Success ? JsonSerializer.SerializeObject(user) : JsonSerializer.SerializeObject(r.Errors);
      };

      await _userUseCase.Read(new ReadUserRequest(id), _readUserPresenter);
      return _readUserPresenter.ContentResult;
    }

    // POST api/<UsersController>
    [HttpPost]
    [Authorize("CanCreateUser")]
    public async Task<ActionResult> Post(Models.Request.CreateUserRequest request)
    {
      if (!ModelState.IsValid)
      { // re-render the view when validation failed.
        return BadRequest(new { Errors= ModelState });
      }
      _createUserPresenter.HandleResource = r =>
      {
        return r.Success ? JsonSerializer.SerializeObject(r.Id) : JsonSerializer.SerializeObject(r);
      };

      var createUserRequest = new Core.Dto.UseCaseRequests.User.CreateUserRequest(request.Username, request.Email, request.FirstName, request.LastName, request.RoleId, request.Password);
      await _userUseCase.Create(createUserRequest, _createUserPresenter);
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
