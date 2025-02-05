namespace BackendService.BusinessLogic.Operations.AddUser.Tasks.HashPassword;

public interface IHashPasswordTask
{
    Task<string> HashAsync(string password, string salt);
}