using BackendService.BusinessLogic.Operations.AddUser;
using BackendService.BusinessLogic.Operations.ChangePassword;
using BackendService.BusinessLogic.Operations.ChangePassword.Models;
using BackendService.Contracts.AddUser;
using BackendService.Contracts.ChangeUserPassword;
using BackendService.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace BackendService.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class InternalController(IAddUserOperation addUserOperation, IChangePasswordOperation changePasswordOperation) : ControllerBase
{
    [HttpPost("AddUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddUserAsync([FromBody] AddUserRequest request)
    {
        await addUserOperation.AddAsync(request.Map()).ConfigureAwait(false);

        return Ok();
    }

    [HttpPost("ChangePassword")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequest request)
    {
        await changePasswordOperation.ChangeAsync(new ChangePasswordOperationRequest(request.Login, request.OldPassword, request.NewPassword)).ConfigureAwait(false);

        return Ok();
    }
}