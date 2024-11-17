namespace BackendService.Contracts.AddUser;

public sealed class AddUserRequest(string login, string password)
{
    public string Login { get; set; } = login;

    public string Password { get; set; } = password;
}