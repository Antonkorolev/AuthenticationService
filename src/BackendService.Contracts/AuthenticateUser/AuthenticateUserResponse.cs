namespace BackendService.Contracts.AuthenticateUser;

public sealed class AuthenticateUserResponse(string token)
{
    public string Token { get; set; } = token;
}