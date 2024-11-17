namespace BackendService.Contracts.AuthenticateUser;

public sealed class AuthenticateUserResponse(bool isAuthenticated)
{
    public bool IsAuthenticated { get; set; } = isAuthenticated;
}