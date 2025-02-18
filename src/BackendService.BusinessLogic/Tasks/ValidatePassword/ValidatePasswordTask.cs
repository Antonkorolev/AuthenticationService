using HashingProvider = BCrypt.Net.BCrypt;

namespace BackendService.BusinessLogic.Tasks.ValidatePassword;

public sealed class ValidatePasswordTask : IValidatePasswordTask
{
    public Task<bool> ValidateAsync(string password, string hash)
    {
        var isVerified = HashingProvider.Verify(password, hash);

        return Task.FromResult(isVerified);
    }
}