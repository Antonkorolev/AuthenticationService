namespace BackendService.Contracts.AddUser;

public sealed class AddUserRequest(string login, string password, Role[] roles)
{
    public string Login { get; set; } = login;

    public string Password { get; set; } = password;

    public Role[] Roles { get; set; } = roles;
}