using BackendService.BusinessLogic.Operations.AddUser.Models;
using BackendService.BusinessLogic.Operations.AddUser.Tasks.GetSalt;
using BackendService.BusinessLogic.Operations.AddUser.Tasks.GetSettings;
using BackendService.BusinessLogic.Operations.AddUser.Tasks.HashPassword;
using BackendService.BusinessLogic.Operations.AddUser.Tasks.UserAddition;
using Microsoft.Extensions.Logging;

namespace BackendService.BusinessLogic.Operations.AddUser;

public sealed class AddUserOperation : IAddUserOperation
{
    private readonly IHashPasswordTask _hashPasswordTask;
    private readonly IUserAdditionTask _userAdditionTask;
    private readonly IGetSaltTask _getSaltTask;
    private readonly IGetSettingsTask _getSettingsTask;
    private readonly ILogger<AddUserOperation> _logger;

    public AddUserOperation(
        IHashPasswordTask hashPasswordTask, 
        IUserAdditionTask userAdditionTask,
        IGetSaltTask getSaltTask,
        IGetSettingsTask getSettingsTask,
        ILogger<AddUserOperation> logger)
    {
        _hashPasswordTask = hashPasswordTask;
        _userAdditionTask = userAdditionTask;
        _getSaltTask = getSaltTask;
        _getSettingsTask = getSettingsTask;
        _logger = logger;
    }

    public async Task AddAsync(AddUserOperationRequest request)
    {
        var getSettingsTaskResponse = await _getSettingsTask.GetAsync().ConfigureAwait(false);
        var salt = await _getSaltTask.GetAsync(getSettingsTaskResponse.WorkFactor, getSettingsTaskResponse.BcryptMinorRevision).ConfigureAwait(false);
        var hash = await _hashPasswordTask.HashAsync(request.Password, salt).ConfigureAwait(false);

        await _userAdditionTask.AddAsync(request.Login, hash, salt).ConfigureAwait(false);

        _logger.LogInformation($"User added");
    }
}