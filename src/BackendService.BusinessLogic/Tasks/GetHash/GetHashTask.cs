using BackendService.BusinessLogic.Exceptions;
using BackendService.BusinessLogic.Tasks.GetHash.Models;
using DatabaseContext.UserDb;
using Microsoft.EntityFrameworkCore;

namespace BackendService.BusinessLogic.Tasks.GetHash;

public sealed class GetHashTask(IUserDbContext dbContext) : IGetHashTask
{
    public async Task<GetHashTaskResponse> GetAsync(string login)
    {
        var user = await dbContext.User.FirstOrDefaultAsync(u => u.Login == login.Trim());
        
        if (user == null)
            throw new UserNotFoundException($"User not found by login: {login}");

        return new GetHashTaskResponse(user.UserId, user.Password);
    }
}