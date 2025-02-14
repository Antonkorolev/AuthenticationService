using BackendService.BusinessLogic.Operations.AddUser;
using BackendService.BusinessLogic.Operations.AddUser.Models;
using BackendService.BusinessLogic.Operations.ChangePassword;
using BackendService.BusinessLogic.Operations.ChangePassword.Models;
using BackendService.Contracts.AddUser;
using BackendService.Contracts.ChangeUserPassword;
using Microsoft.AspNetCore.Mvc;

namespace BackendService.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class InternalController : ControllerBase
{
    private readonly IAddUserOperation _addUserOperation;
    private readonly IChangePasswordOperation _changePasswordOperation;

    public InternalController(IAddUserOperation addUserOperation, IChangePasswordOperation changePasswordOperation)
    {
        _addUserOperation = addUserOperation;
        _changePasswordOperation = changePasswordOperation;
    }

    [HttpPost("AddUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddUserAsync([FromBody] AddUserRequest request)
    {
        await _addUserOperation.AddAsync(new AddUserOperationRequest(request.Login, request.Password)).ConfigureAwait(false);

        return Ok();
    }

    [HttpPost("ChangePassword")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequest request)
    {
        await _changePasswordOperation.ChangeAsync(new ChangePasswordOperationRequest(request.Login, request.OldPassword, request.NewPassword)).ConfigureAwait(false);

        return Ok();
    }
}