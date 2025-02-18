using BackendService.BusinessLogic.Operations.AddUser.Models;
using BackendService.BusinessLogic.Operations.AddUser.Tasks.UserAddition;
using BackendService.BusinessLogic.Tasks.GetSalt;
using BackendService.BusinessLogic.Tasks.GetSettings;
using BackendService.BusinessLogic.Tasks.HashPassword;
using Microsoft.Extensions.Logging;

namespace BackendService.BusinessLogic.Operations.AddUser;

public sealed class AddUserOperation(
    IHashPasswordTask hashPasswordTask,
    IUserAdditionTask userAdditionTask,
    IGetSaltTask getSaltTask,
    IGetSettingsTask getSettingsTask,
    ILogger<AddUserOperation> logger)
    : IAddUserOperation
{
    public async Task AddAsync(AddUserOperationRequest request)
    {
        var getSettingsTaskResponse = await getSettingsTask.GetAsync().ConfigureAwait(false);
        var salt = await getSaltTask.GetAsync(getSettingsTaskResponse.WorkFactor, getSettingsTaskResponse.BcryptMinorRevision).ConfigureAwait(false);
        var hash = await hashPasswordTask.HashAsync(request.Password, salt).ConfigureAwait(false);

        await userAdditionTask.AddAsync(request.Login, hash, salt).ConfigureAwait(false);

        logger.LogInformation($"User added");
    }
}