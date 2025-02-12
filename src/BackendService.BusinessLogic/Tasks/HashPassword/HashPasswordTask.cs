using HashingProvider = BCrypt.Net.BCrypt;

namespace BackendService.BusinessLogic.Tasks.HashPassword;

public sealed class HashPasswordTask : IHashPasswordTask
{
    public Task<string> HashAsync(string password, string salt)
    {
        var result = HashingProvider.HashPassword(password, salt);

        return Task.FromResult(result);
    }
}