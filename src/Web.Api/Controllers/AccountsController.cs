﻿using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses.Account;
using Web.Api.Core.Dto.UseCaseResponses.User;
using Web.Api.Core.Interfaces.Services;
using Web.Api.Core.Interfaces.UseCases;
using Web.Api.Core.Interfaces.UseCases.Account;
using Web.Api.Models.Request;
using Web.Api.Presenters;
using Web.Api.Serialization;

namespace Web.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AccountsController : ControllerBase
  {
    private readonly IUpdateProfileInfoUseCase _updateProfileInfoUseCase;
    private readonly IChangePasswordUseCase _changePasswordUseCase;
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    private readonly ResourcePresenter<UpdateProfileInfoResponse> _updateProfileInfoPresenter;
    private readonly ResourcePresenter<ChangePasswordResponse> _changePasswordPresenter;

    public AccountsController(
      IUpdateProfileInfoUseCase updateProfileInfoUseCase,
      IChangePasswordUseCase changePasswordUseCase,
      IAuthService authService,
      IMapper mapper,
      ResourcePresenter<UpdateProfileInfoResponse> updateProfileInfoPresenter,
      ResourcePresenter<ChangePasswordResponse> changePasswordPresenter
    )
    {
      _authService = authService;
      _mapper = mapper;
      _updateProfileInfoUseCase = updateProfileInfoUseCase;
      _updateProfileInfoPresenter = updateProfileInfoPresenter;
      _changePasswordUseCase = changePasswordUseCase;
      _changePasswordPresenter = changePasswordPresenter;
    }

    [HttpGet("me")]
    [Authorize()]
    public ActionResult<object> CurrentUser()
    {
      var user = _authService.GetCurrentUser();

      return new
      {
        user.Id,
        user.FirstName,
        user.LastName,
        user.Account.Username,
        user.RoleId,
        user.Email,
        Role = user.Role.Name,
        Permissions = _authService.GetAllPermissions()
      };
    }

    [HttpPut("profile/info")]
    [Authorize()]
    public async Task<ActionResult> UpdateProfileInfo(UpdateProfileInfoRequest request)
    {
      _updateProfileInfoPresenter.HandleResource = r =>
      {
        return r.Success ? JsonSerializer.SerializeObject(new { r.UpdatedFields }) : JsonSerializer.SerializeObject(new { r.Errors });
      };
      var user = _authService.GetCurrentUser();
      await _updateProfileInfoUseCase.Handle(
        new Core.Dto.UseCaseRequests.Account.UpdateProfileInfoRequest(user, request.FirstName, request.LastName),
        _updateProfileInfoPresenter);

      return _updateProfileInfoPresenter.ContentResult;
    }

    [HttpPut("profile/password")]
    [Authorize()]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
      var user = _authService.GetCurrentUser();
      _changePasswordPresenter.HandleResource = r => r.Success ? "" : JsonSerializer.SerializeObject(new {r.Errors});

      var response = await _changePasswordUseCase.Handle(
        new Core.Dto.UseCaseRequests.Account.ChangePasswordRequest(user, request.CurrentPassword, request.NewPassword),
        _changePasswordPresenter
      );
      
      if (!response) return _changePasswordPresenter.ContentResult;
      
      return _changePasswordPresenter.ContentResult;
    }
  } 
}
