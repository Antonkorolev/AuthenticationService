namespace BackendService.BusinessLogic.Operations.AuthenticateUser.Models;

public sealed class AuthenticateUserOperationRequest(string login, string password)
{
    public string Login { get; set; } = login;

    public string Password { get; set; } = password;
}