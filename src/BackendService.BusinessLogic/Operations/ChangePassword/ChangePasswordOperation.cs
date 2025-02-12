using BackendService.BusinessLogic.Exceptions;
using BackendService.BusinessLogic.Operations.ChangePassword.Models;
using BackendService.BusinessLogic.Tasks.GetSalt;
using BackendService.BusinessLogic.Tasks.GetSettings;
using BackendService.BusinessLogic.Tasks.HashPassword;
using BackendService.BusinessLogic.Tasks.ValidateUser;
using DatabaseContext.UserDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackendService.BusinessLogic.Operations.ChangePassword;

public sealed class ChangePasswordOperation : IChangePasswordOperation
{
    private readonly IUserDbContext _dbContext;
    private readonly IHashPasswordTask _hashPasswordTask;
    private readonly IValidateUserTask _validateUserTask;
    private readonly IGetSaltTask _getSaltTask;
    private readonly IGetSettingsTask _getSettingsTask;
    private readonly ILogger<ChangePasswordOperation> _logger;

    public ChangePasswordOperation(
        IUserDbContext dbContext, 
        IHashPasswordTask hashPasswordTask, 
        IValidateUserTask validateUserTask, 
        IGetSaltTask getSaltTask, 
        IGetSettingsTask getSettingsTask, 
        ILogger<ChangePasswordOperation> logger)
    {
        _dbContext = dbContext;
        _hashPasswordTask = hashPasswordTask;
        _validateUserTask = validateUserTask;
        _getSaltTask = getSaltTask;
        _getSettingsTask = getSettingsTask;
        _logger = logger;
    }

    public async Task ChangeAsync(ChangePasswordOperationRequest request)
    {
        var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Login == request.Login.Trim()).ConfigureAwait(false);
        var isVerified = await _validateUserTask.ValidateAsync(user, request.Login, request.OldPassword).ConfigureAwait(false);

        if (!isVerified)
            throw new UserVerifiedException("User not verified");
        
        var getSettingsTaskResponse = await _getSettingsTask.GetAsync().ConfigureAwait(false);
        var salt = await _getSaltTask.GetAsync(getSettingsTaskResponse.WorkFactor, getSettingsTaskResponse.BcryptMinorRevision).ConfigureAwait(false);
        var hash = await _hashPasswordTask.HashAsync(request.NewPassword, salt).ConfigureAwait(false);

        user.Password = hash;
        user.Salt = salt;
        
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        
        _logger.LogInformation($"Password successfully changed {isVerified}");
    }
}