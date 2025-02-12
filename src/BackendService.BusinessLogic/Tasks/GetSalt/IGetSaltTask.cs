namespace BackendService.BusinessLogic.Tasks.GetSalt;

public interface IGetSaltTask
{
    Task<string> GetAsync(int workFactor, char bcryptMinorRevision);
}