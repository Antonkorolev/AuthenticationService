using BackendService.BusinessLogic.Operations.AddUser.Tasks.GetSettings.Models;

namespace BackendService.BusinessLogic.Operations.AddUser.Tasks.GetSettings;

public interface IGetSettingsTask
{
    Task<GetSettingsTaskResponse> GetAsync();
}