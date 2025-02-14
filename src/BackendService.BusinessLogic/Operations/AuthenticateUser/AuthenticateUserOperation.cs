using BackendService.BusinessLogic.Operations.AuthenticateUser.Models;
using BackendService.BusinessLogic.Tasks.GetHash;
using BackendService.BusinessLogic.Tasks.ValidateUser;
using Microsoft.Extensions.Logging;

namespace BackendService.BusinessLogic.Operations.AuthenticateUser;

public sealed class AuthenticateUserOperation : IAuthenticateUserOperation
{
    private readonly IValidateUserTask _validateUserTask;
    private readonly IGetHashTask _getHashTask;
    private readonly ILogger<AuthenticateUserOperation> _logger;

    public AuthenticateUserOperation(
        IValidateUserTask validateUserTask,
        IGetHashTask getHashTask,
        ILogger<AuthenticateUserOperation> logger)
    {
        _validateUserTask = validateUserTask;
        _getHashTask = getHashTask;
        _logger = logger;
    }

    public async Task<AuthenticateUserOperationResponse> AuthenticateAsync(AuthenticateUserOperationRequest request)
    {
        var hash = await _getHashTask.GetAsync(request.Login).ConfigureAwait(false);
        var isVerified = await _validateUserTask.ValidateAsync(request.Password, hash).ConfigureAwait(false);

        _logger.LogInformation($"User authentication is {isVerified}");

        return new AuthenticateUserOperationResponse(isVerified);
    }
}