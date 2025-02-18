using BackendService.BusinessLogic.Operations.AuthenticateUser.Models;
using BackendService.BusinessLogic.Tasks.GetHash;
using BackendService.BusinessLogic.Tasks.ValidatePassword;
using Microsoft.Extensions.Logging;

namespace BackendService.BusinessLogic.Operations.AuthenticateUser;

public sealed class AuthenticateUserOperation(
    IValidatePasswordTask validatePasswordTask,
    IGetHashTask getHashTask,
    ILogger<AuthenticateUserOperation> logger) : IAuthenticateUserOperation
{
    public async Task<AuthenticateUserOperationResponse> AuthenticateAsync(AuthenticateUserOperationRequest request)
    {
        var hash = await getHashTask.GetAsync(request.Login).ConfigureAwait(false);
        var isVerified = await validatePasswordTask.ValidateAsync(request.Password, hash).ConfigureAwait(false);

        logger.LogInformation($"User authentication is {isVerified}");

        return new AuthenticateUserOperationResponse(isVerified);
    }
}