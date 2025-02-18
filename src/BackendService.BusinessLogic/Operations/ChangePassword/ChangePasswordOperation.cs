using BackendService.BusinessLogic.Operations.ChangePassword.Models;
using BackendService.BusinessLogic.Operations.ChangePassword.Tasks.ChangePassword;
using BackendService.BusinessLogic.Operations.ChangePassword.Tasks.ChangePassword.Models;
using BackendService.BusinessLogic.Operations.ChangePassword.Tasks.ValidateOldPassword;
using BackendService.BusinessLogic.Operations.ChangePassword.Tasks.ValidateOldPassword.Models;
using Microsoft.Extensions.Logging;

namespace BackendService.BusinessLogic.Operations.ChangePassword;

public sealed class ChangePasswordOperation(
    IValidateOldPasswordTask validateOldPasswordTask,
    IChangePasswordTask changePasswordTask,
    ILogger<ChangePasswordOperation> logger)
    : IChangePasswordOperation
{
    public async Task ChangeAsync(ChangePasswordOperationRequest request)
    {
        await validateOldPasswordTask.ValidateAsync(new ValidateOldPasswordTaskRequest(request.Login, request.OldPassword)).ConfigureAwait(false);
        await changePasswordTask.ChangeAsync(new ChangePasswordRequest(request.Login, request.NewPassword)).ConfigureAwait(false);

        logger.LogInformation("Password successfully changed");
    }
}