namespace BackendService.BusinessLogic.Tasks.ValidatePassword;

public interface IValidatePasswordTask
{
    Task<bool> ValidateAsync(string password, string hash);
}