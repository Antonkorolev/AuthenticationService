using BackendService.BusinessLogic.Operations.AuthenticateUser;
using BackendService.BusinessLogic.Operations.AuthenticateUser.Models;
using BackendService.Contracts.AuthenticateUser;
using Microsoft.AspNetCore.Mvc;

namespace BackendService.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IAuthenticateUserOperation _authenticateUserOperation;

    public UserController(IAuthenticateUserOperation authenticateUserOperation)
    {
        _authenticateUserOperation = authenticateUserOperation;
    }

    [HttpPost("Authenticate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AuthenticateUserAsync([FromBody] AuthenticateUserRequest request)
    {
        var result = await _authenticateUserOperation.AuthenticateAsync(new AuthenticateUserOperationRequest(request.Login, request.Password)).ConfigureAwait(false);

        return Ok(new AuthenticateUserResponse(result.IsAuthenticated));
    }
}