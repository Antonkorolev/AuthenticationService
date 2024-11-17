using BackendService.BusinessLogic.Operations.AuthenticateUser.Models;
using BackendService.BusinessLogic.Operations.AuthenticateUser.Tasks.ValidateUser;
using DatabaseContext.UserDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackendService.BusinessLogic.Operations.AuthenticateUser;

public sealed class AuthenticateUserOperation : IAuthenticateUserOperation
{
    private readonly IUserDbContext _dbContext;
    private readonly IValidateUserTask _validateUserTask;
    private readonly ILogger<AuthenticateUserOperation> _logger;

    public AuthenticateUserOperation(IUserDbContext dbContext, IValidateUserTask validateUserTask, ILogger<AuthenticateUserOperation> logger)
    {
        _dbContext = dbContext;
        _validateUserTask = validateUserTask;
        _logger = logger;
    }

    public async Task<AuthenticateUserOperationResponse> AuthenticateAsync(AuthenticateUserOperationRequest request)
    {
        var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Login == request.Login.Trim()).ConfigureAwait(false);

        var isVerified = await _validateUserTask.ValidateAsync(user, request.Login, request.Password).ConfigureAwait(false);

        _logger.LogInformation($"User authentication is {isVerified}");

        return new AuthenticateUserOperationResponse(isVerified);
    }
}