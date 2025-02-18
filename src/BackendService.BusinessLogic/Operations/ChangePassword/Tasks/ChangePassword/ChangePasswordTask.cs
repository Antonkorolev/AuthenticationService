using BackendService.BusinessLogic.Operations.ChangePassword.Tasks.ChangePassword.Models;
using BackendService.BusinessLogic.Tasks.GetSalt;
using BackendService.BusinessLogic.Tasks.GetSettings;
using BackendService.BusinessLogic.Tasks.HashPassword;
using DatabaseContext.UserDb;
using Microsoft.EntityFrameworkCore;

namespace BackendService.BusinessLogic.Operations.ChangePassword.Tasks.ChangePassword;

public sealed class ChangePasswordTask(
    UserDbContext dbContext,
    IGetSettingsTask getSettingsTask,
    IGetSaltTask getSaltTask,
    IHashPasswordTask hashPasswordTask) : IChangePasswordTask
{
    public async Task ChangeAsync(ChangePasswordRequest request)
    {
        var getSettingsTaskResponse = await getSettingsTask.GetAsync().ConfigureAwait(false);
        var newSalt = await getSaltTask.GetAsync(getSettingsTaskResponse.WorkFactor, getSettingsTaskResponse.BcryptMinorRevision).ConfigureAwait(false);
        var newHash = await hashPasswordTask.HashAsync(request.Password, newSalt).ConfigureAwait(false);

        var user = await dbContext.User.FirstAsync(u => u.Login == request.Login.Trim());

        user.Password = newHash;
        user.Salt = newSalt;

        await dbContext.SaveChangesAsync(CancellationToken.None).ConfigureAwait(false);
    }
}