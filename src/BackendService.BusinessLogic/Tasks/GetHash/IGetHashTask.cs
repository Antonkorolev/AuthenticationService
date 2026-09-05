using BackendService.BusinessLogic.Tasks.GetHash.Models;

namespace BackendService.BusinessLogic.Tasks.GetHash;

public interface IGetHashTask
{
    Task<GetHashTaskResponse> GetAsync(string login);
}