using BackendService.BusinessLogic.Exceptions;
using BackendService.BusinessLogic.Operations.ChangePassword.Models;
using BackendService.BusinessLogic.Operations.ChangePassword.Tasks.ChangePassword;
using BackendService.BusinessLogic.Tasks.GetHash;
using BackendService.BusinessLogic.Tasks.GetSalt;
using BackendService.BusinessLogic.Tasks.GetSettings;
using BackendService.BusinessLogic.Tasks.HashPassword;
using BackendService.BusinessLogic.Tasks.ValidateUser;
using Microsoft.Extensions.Logging;

namespace BackendService.BusinessLogic.Operations.ChangePassword;

public sealed class ChangePasswordOperation : IChangePasswordOperation
{
    private readonly IGetHashTask _getHashTask;
    private readonly IHashPasswordTask _hashPasswordTask;
    private readonly IValidateUserTask _validateUserTask;
    private readonly IGetSaltTask _getSaltTask;
    private readonly IGetSettingsTask _getSettingsTask;
    private readonly IChangePasswordTask _changePasswordTask;
    private readonly ILogger<ChangePasswordOperation> _logger;

    public ChangePasswordOperation(
        IGetHashTask getHashTask,
        IHashPasswordTask hashPasswordTask,
        IValidateUserTask validateUserTask,
        IGetSaltTask getSaltTask,
        IGetSettingsTask getSettingsTask,
        IChangePasswordTask changePasswordTask,
        ILogger<ChangePasswordOperation> logger)
    {
        _getHashTask = getHashTask;
        _hashPasswordTask = hashPasswordTask;
        _validateUserTask = validateUserTask;
        _getSaltTask = getSaltTask;
        _getSettingsTask = getSettingsTask;
        _changePasswordTask = changePasswordTask;
        _logger = logger;
    }

    public async Task ChangeAsync(ChangePasswordOperationRequest request)
    {
        var oldHash = await _getHashTask.GetAsync(request.Login).ConfigureAwait(false);
        var isVerified = await _validateUserTask.ValidateAsync(request.OldPassword, oldHash).ConfigureAwait(false);

        if (!isVerified)
            throw new UserVerifiedException("User not verified");

        var getSettingsTaskResponse = await _getSettingsTask.GetAsync().ConfigureAwait(false);
        var newSalt = await _getSaltTask.GetAsync(getSettingsTaskResponse.WorkFactor, getSettingsTaskResponse.BcryptMinorRevision).ConfigureAwait(false);
        var newHash = await _hashPasswordTask.HashAsync(request.NewPassword, newSalt).ConfigureAwait(false);

        await _changePasswordTask.ChangeAsync(request.Login, newHash, newSalt).ConfigureAwait(false);

        _logger.LogInformation("Password successfully changed");
    }
}