namespace BackendService.BusinessLogic.Operations.ChangePassword.Tasks.ChangePassword;

public interface IChangePasswordTask
{
   Task ChangeAsync(string login, string hash, string salt);
}