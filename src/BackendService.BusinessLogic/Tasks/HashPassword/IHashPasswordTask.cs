namespace BackendService.BusinessLogic.Tasks.HashPassword;

public interface IHashPasswordTask
{
    Task<string> HashAsync(string password, string salt);
}