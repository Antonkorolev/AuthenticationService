using BackendService.BusinessLogic.Tasks.CreateJwtToken.Models;

namespace BackendService.BusinessLogic.Tasks.CreateJwtToken;

public interface ICreateJwtTokenTask
{
    Task<string> CreateAsync(CreateJwtTokenTaskRequest request);
}