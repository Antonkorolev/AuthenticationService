using HashingProvider = BCrypt.Net.BCrypt;

namespace BackendService.BusinessLogic.Operations.AddUser.Tasks.HashPassword;

public sealed class HashPasswordTask : IHashPasswordTask
{
    public Task<string> HashAsync(string password)
    {
        var result = HashingProvider.HashPassword(password);

        return Task.FromResult(result);
    }
}