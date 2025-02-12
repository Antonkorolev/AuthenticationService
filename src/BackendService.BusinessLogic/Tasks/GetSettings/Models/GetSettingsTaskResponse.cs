namespace BackendService.BusinessLogic.Tasks.GetSettings.Models;

public sealed class GetSettingsTaskResponse(int workFactor, char bcryptMinorRevision)
{
    public int WorkFactor { get; set; } = workFactor;

    public char BcryptMinorRevision { get; set; } = bcryptMinorRevision;
}