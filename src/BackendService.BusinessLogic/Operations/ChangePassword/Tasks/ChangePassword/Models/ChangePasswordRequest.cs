namespace BackendService.BusinessLogic.Operations.ChangePassword.Tasks.ChangePassword.Models;

public sealed class ChangePasswordRequest(string login, string password)
{
    public string Login { get; set; } = login;

    public string Password { get; set; } = password;
}