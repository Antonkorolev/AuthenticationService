using BackendService.BusinessLogic.Exceptions;
using DatabaseContext.UserDb.Models;
using HashingProvider = BCrypt.Net.BCrypt;

namespace BackendService.BusinessLogic.Operations.AuthenticateUser.Tasks.ValidateUser;

public sealed class ValidateUserTask : IValidateUserTask
{
    public Task<bool> ValidateAsync(User? user, string login, string password)
    {
        if (user == null)
            throw new UserNotFoundException($"User not found by login: {login}");

        var isVerified = HashingProvider.Verify(password, user.Password);

        return Task.FromResult(isVerified);
    }
}