using BackendService.BusinessLogic.Operations.AddUser;
using BackendService.BusinessLogic.Operations.AddUser.Models;
using BackendService.Contracts.AddUser;
using Microsoft.AspNetCore.Mvc;

namespace BackendService.Controllers;

[ApiController]
[Route("[controller]")]
public class SupportController : ControllerBase
{
    private readonly IAddUserOperation _addUserOperation;

    public SupportController(IAddUserOperation addUserOperation)
    {
        _addUserOperation = addUserOperation;
    }

    [HttpPost("AddUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddUserAsync([FromBody]AddUserRequest request)
    {
        await _addUserOperation.AddAsync(new AddUserOperationRequest(request.Login, request.Password)).ConfigureAwait(false);

        return Ok();
    }
}