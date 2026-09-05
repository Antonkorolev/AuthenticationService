namespace BackendService.BusinessLogic.Tasks.CreateJwtToken.Models;

public sealed class RoleInfo(string role, IEnumerable<Permission> permissions)
{
    public string RoleCode { get; } = role;

    public IEnumerable<Permission> Permissions { get; } = permissions;
}