namespace BackendService.Contracts.AddUser;

public sealed class Role(string roleCode, string? restrictionType, string? restrictionValue)
{
    public string RoleCode { get; set; } = roleCode;

    public string? RestrictionType { get; set; } = restrictionType;

    public string? RestrictionValue { get; set; } = restrictionValue;
}