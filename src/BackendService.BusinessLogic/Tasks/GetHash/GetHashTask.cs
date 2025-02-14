using BackendService.BusinessLogic.Exceptions;
using DatabaseContext.UserDb;

namespace BackendService.BusinessLogic.Tasks.GetHash;

public sealed class GetHashTask : IGetHashTask
{
    private readonly IUserDbContext _dbContext;

    public GetHashTask(IUserDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<string> GetAsync(string login)
    {
        var user = _dbContext.User.FirstOrDefault(u => u.Login == login.Trim());
        
        if (user == null)
            throw new UserNotFoundException($"User not found by login: {login}");

        return Task.FromResult(user.Password);
    }
}