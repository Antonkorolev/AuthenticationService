using DatabaseContext.UserDb;
using DatabaseContext.UserDb.Models;

namespace BackendService.BusinessLogic.Operations.AddUser.Tasks.UserAddition;

public sealed class UserAdditionTask(IUserDbContext dbContext) : IUserAdditionTask
{
    public async Task AddAsync(string login, string hash, string salt)
    {
        await dbContext.User.AddAsync(new User { Login = login, Password = hash, Salt = salt});

        await dbContext.SaveChangesAsync(CancellationToken.None).ConfigureAwait(false);
    }
}