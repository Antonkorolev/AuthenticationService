using DatabaseContext.UserDb;
using DatabaseContext.UserDb.Models;

namespace BackendService.BusinessLogic.Operations.AddUser.Tasks.UserAddition;

public class UserAdditionTask : IUserAdditionTask
{
    private readonly IUserDbContext _dbContext;

    public UserAdditionTask(IUserDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(string login, string hash, string salt)
    {
        await _dbContext.User.AddAsync(new User { Login = login, Password = hash, Salt = salt});

        await _dbContext.SaveChangesAsync(CancellationToken.None).ConfigureAwait(false);
    }
}