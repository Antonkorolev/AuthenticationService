namespace BackendService.BusinessLogic.Tasks.GetSalt;

public sealed class GetSaltTask : IGetSaltTask
{
    public Task<string> GetAsync(int workFactor, char bcryptMinorRevision)
    {
        return Task.FromResult(BCrypt.Net.BCrypt.GenerateSalt(workFactor, bcryptMinorRevision));
    }
}