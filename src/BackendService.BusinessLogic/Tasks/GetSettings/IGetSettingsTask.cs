using BackendService.BusinessLogic.Tasks.GetSettings.Models;

namespace BackendService.BusinessLogic.Tasks.GetSettings;

public interface IGetSettingsTask
{
    Task<GetSettingsTaskResponse> GetAsync();
}