namespace BackendService.BusinessLogic.Operations.AddUser.Tasks.GetSalt;

public interface IGetSaltTask
{
    Task<string> GetAsync(int workFactor, char bcryptMinorRevision);
}