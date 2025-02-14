namespace BackendService.BusinessLogic.Tasks.GetHash;

public interface IGetHashTask
{
    Task<string> GetAsync(string login);
}