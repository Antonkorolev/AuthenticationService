using BackendService.BusinessLogic.Exceptions;
using BackendService.BusinessLogic.Tasks.GetSettings.Models;
using DatabaseContext.UserDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackendService.BusinessLogic.Tasks.GetSettings;

public sealed class GetSettingsTask : IGetSettingsTask
{
    private const string WorkFactor = "WorkFactor";
    private const string BcryptMinorRevision = "BcryptMinorRevision";
    
    private readonly IUserDbContext _userDbContext;
    private readonly ILogger<GetSettingsTask> _logger;

    public GetSettingsTask(IUserDbContext userDbContext, ILogger<GetSettingsTask> logger)
    {
        _userDbContext = userDbContext;
        _logger = logger;
    }
    
    public async Task<GetSettingsTaskResponse> GetAsync()
    {
        var settings = await _userDbContext.Settings.ToArrayAsync().ConfigureAwait(false);

        var workFactor = settings.FirstOrDefault(s => s.Key == WorkFactor)?.Value;

        if (workFactor == null)
            throw new SettingNotFoundException(WorkFactor);
        
        var bcryptMinorRevision = settings.FirstOrDefault(s => s.Key == BcryptMinorRevision)?.Value;
        
        if (bcryptMinorRevision == null)
            throw new SettingNotFoundException(BcryptMinorRevision);
        
        _logger.LogInformation($"Settings for {WorkFactor} and {BcryptMinorRevision} successfully received");
        
        return new GetSettingsTaskResponse(int.Parse(workFactor), char.Parse(bcryptMinorRevision));
    }
}