using DatabaseContext.UserDb.Models;
using HashingProvider = BCrypt.Net.BCrypt;

namespace BackendService.BusinessLogic.Tasks.ValidateUser;

public sealed class ValidateUserTask : IValidateUserTask
{
    public Task<bool> ValidateAsync(string password, string hash)
    {
        var isVerified = HashingProvider.Verify(password, hash);

        return Task.FromResult(isVerified);
    }
}