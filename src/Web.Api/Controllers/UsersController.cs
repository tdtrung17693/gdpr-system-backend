using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Dto.UseCaseRequests.User;
using Web.Api.Core.UseCases.User;
using Web.Api.Presenters;
using Web.Api.Core.Dto.UseCaseResponses.User;
using Web.Api.Serialization;
using Web.Api.Models.Request;
using AutoMapper;
using Web.Api.Core.Dto;
using System;
using Web.Api.Core.Interfaces.Services.Event;
using Web.Api.Core.Domain.Event;
using Web.Api.Core.Dto.UseCaseRequests;

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
        private ResourcePresenter<ChangeUsersStatusResponse> _changeUsersStatusPresenter;
        private ResourcePresenter<UpdateUserResponse> _updateUserPresenter;
        private IMapper _mapper;

        public UsersController(
            ManageUserUseCase userUseCase,
            ResourcePresenter<ReadUserResponse> readUserPresenter,
            ResourcePresenter<CreateUserResponse> createUserPresenter,
            ResourcePresenter<UpdateUserResponse> updateUserPresenter,
            ResourcePresenter<ChangeUsersStatusResponse> changeUsersStatusPresenter,
            IMapper mapper)
        {
            _userUseCase = userUseCase;
            _readUserPresenter = readUserPresenter;
            _createUserPresenter = createUserPresenter;
            _changeUsersStatusPresenter = changeUsersStatusPresenter;
            _updateUserPresenter = updateUserPresenter;
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
        public async Task<ActionResult> Get(Guid id)
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
            {
                // re-render the view when validation failed.
                return BadRequest(new {Errors = ModelState});
            }

            _createUserPresenter.HandleResource = r =>
            {
                return r.Success
                    ? JsonSerializer.SerializeObject(r.Id)
                    : JsonSerializer.SerializeObject(r.Errors.First());
            };

            var createUserRequest = new Core.Dto.UseCaseRequests.User.CreateUserRequest(request.Username, request.Email,
                request.FirstName, request.LastName, Guid.Parse(request.RoleId));
            await _userUseCase.Create(createUserRequest, _createUserPresenter);
            return _createUserPresenter.ContentResult;
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        [Authorize("CanEditUser")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Models.Request.UpdateUserRequest request)
        {
            _updateUserPresenter.HandleResource = r =>
            {
                return r.Success ? "" : JsonSerializer.SerializeObject(r.Errors);
            };

            var updateUserRequest =
                new Core.Dto.UseCaseRequests.User.UpdateUserRequest(id, request.RoleId, request.Status);
            await _userUseCase.Update(updateUserRequest, _updateUserPresenter);
            //return _updateUserPresenter.ContentResult.ToString() == "" ? (IActionResult) NoContent() : BadRequest(_updateUserPresenter.ContentResult.ToString());
            return _updateUserPresenter.ContentResult;
        }

        [HttpPost("activate")]
        public async Task<ActionResult> ActivateBulk([FromBody] Guid[] ids)
        {
            _changeUsersStatusPresenter.HandleResource = r =>
            {
                return r.Success
                    ? JsonSerializer.SerializeObject(new { })
                    : JsonSerializer.SerializeObject(r.Errors);
            };
            await _userUseCase.ChangeUserStatus(new ChangeUsersStatusRequest(ids, true), _changeUsersStatusPresenter);
            return _changeUsersStatusPresenter.ContentResult;
        }

        [HttpPost("deactivate")]
        [Authorize("CanEditUser")]
        public async Task<ActionResult> DeactivateBulk([FromBody] Guid[] ids)
        {
            _changeUsersStatusPresenter.HandleResource = r =>
            {
                return r.Success
                    ? JsonSerializer.SerializeObject(new { })
                    : JsonSerializer.SerializeObject(r.Errors);
            };
            await _userUseCase.ChangeUserStatus(new ChangeUsersStatusRequest(ids, false), _changeUsersStatusPresenter);
            return _changeUsersStatusPresenter.ContentResult;
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        [Authorize("CanDeleteUser")]
        public void Delete(int id)
        {
        }

        [HttpPost("testevent")]
        public async Task TestEvent([FromServices] IDomainEventBus eventBus, [FromServices] IUserRepository repo)
        {
            var user = await repo.FindById(Guid.Parse("61662330-eb32-47d7-a680-5f2c47a5ca60"));
            await eventBus.Trigger(new UserCreated(user.FirstName, user.LastName, "ABC", user.Email,
                user.Account.Username));
        }

        //Khoa
        [HttpPost("avatar")]
        public async Task<ActionResult> UploadFirstAvatar([FromBody] UploadAvatarRequest request)
        {
            if (!ModelState.IsValid)
            {
                // re-render the view when validation failed.
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}