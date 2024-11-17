using BackendService.BusinessLogic.Operations.AddUser.Models;
using BackendService.BusinessLogic.Operations.AddUser.Tasks.HashPassword;
using BackendService.BusinessLogic.Operations.AddUser.Tasks.UserAddition;
using Microsoft.Extensions.Logging;

namespace BackendService.BusinessLogic.Operations.AddUser;

public sealed class AddUserOperation : IAddUserOperation
{
    private readonly IHashPasswordTask _hashPasswordTask;
    private readonly IUserAdditionTask _userAdditionTask;
    private readonly ILogger<AddUserOperation> _logger;

    public AddUserOperation(IHashPasswordTask hashPasswordTask, IUserAdditionTask userAdditionTask, ILogger<AddUserOperation> logger)
    {
        _hashPasswordTask = hashPasswordTask;
        _userAdditionTask = userAdditionTask;
        _logger = logger;
    }

    public async Task AddAsync(AddUserOperationRequest request)
    {
        var hash = await _hashPasswordTask.HashAsync(request.Password).ConfigureAwait(false);

        await _userAdditionTask.AddAsync(request.Login, hash).ConfigureAwait(false);

        _logger.LogInformation($"User added");
    }
}