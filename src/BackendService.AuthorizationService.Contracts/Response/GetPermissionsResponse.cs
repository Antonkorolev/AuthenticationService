namespace BackendService.AuthorizationService.Contracts.Response;

public sealed class GetPermissionsResponse(IEnumerable<RolePermission> rolePermissions)
{
    public IEnumerable<RolePermission> RolePermissions { get; } = rolePermissions;
}