using DatabaseContext.UserDb;
using Microsoft.EntityFrameworkCore;

namespace BackendService.BusinessLogic.Operations.ChangePassword.Tasks.ChangePassword;

public sealed class ChangePasswordTask : IChangePasswordTask
{
    private readonly UserDbContext _dbContext;

    public ChangePasswordTask(UserDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ChangeAsync(string login, string hash, string salt)
    {
        var user = await _dbContext.User.FirstAsync(u => u.Login == login.Trim());

        user.Password = hash;
        user.Salt = salt;

        await _dbContext.SaveChangesAsync(CancellationToken.None).ConfigureAwait(false);
    }
}