namespace BackendService.BusinessLogic.Operations.AddUser.Models;

public sealed class AddUserOperationRequest(string login, string password, RoleInfo[] roleInfos)
{
    public string Login { get; set; } = login;

    public string Password { get; set; } = password;

    public RoleInfo[] Roles = roleInfos;
}