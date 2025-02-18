namespace BackendService.BusinessLogic.Operations.ChangePassword.Tasks.ValidateOldPassword.Models;

public sealed class ValidateOldPasswordTaskRequest(string login, string password)
{
    public string Login { get; set; } = login;

    public string Password { get; set; } = password;
}