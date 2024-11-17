namespace BackendService.Contracts.AuthenticateUser;

public sealed class AuthenticateUserRequest(string login, string password)
{
    public string Login { get; set; } = login;

    public string Password { get; set; } = password;
}