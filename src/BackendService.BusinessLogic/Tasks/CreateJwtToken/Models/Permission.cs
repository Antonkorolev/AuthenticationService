namespace BackendService.BusinessLogic.Tasks.CreateJwtToken.Models;

public sealed class Permission(string permissionCode, string? restrictionTypeCode, string? restrictionValue)
{
    public string PermissionCode { get; } = permissionCode;
    
    public string? RestrictionTypeCode { get; } = restrictionTypeCode;

    public string? RestrictionValue { get; } = restrictionValue;
}