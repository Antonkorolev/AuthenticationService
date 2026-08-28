namespace BackendService.AuthorizationService.Contracts.Response;

public sealed class Restriction(string? restrictionTypeCode, string? restrictionValue)
{
    public string? RestrictionTypeCode { get; set; } = restrictionTypeCode;

    public string? RestrictionValue { get; set; } = restrictionValue;
}