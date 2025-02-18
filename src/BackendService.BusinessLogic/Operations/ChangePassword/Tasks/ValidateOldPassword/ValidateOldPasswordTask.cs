using BackendService.BusinessLogic.Exceptions;
using BackendService.BusinessLogic.Operations.ChangePassword.Tasks.ValidateOldPassword.Models;
using BackendService.BusinessLogic.Tasks.GetHash;
using BackendService.BusinessLogic.Tasks.ValidatePassword;

namespace BackendService.BusinessLogic.Operations.ChangePassword.Tasks.ValidateOldPassword;

public sealed class ValidateOldPasswordTask(IGetHashTask getHashTask, IValidatePasswordTask validatePasswordTask) : IValidateOldPasswordTask
{
    public async Task ValidateAsync(ValidateOldPasswordTaskRequest request)
    {
        var oldHash = await getHashTask.GetAsync(request.Login).ConfigureAwait(false);
        var isVerified = await validatePasswordTask.ValidateAsync(request.Password, oldHash).ConfigureAwait(false);

        if (!isVerified)
            throw new UserVerifiedException("User not verified");
    }
}