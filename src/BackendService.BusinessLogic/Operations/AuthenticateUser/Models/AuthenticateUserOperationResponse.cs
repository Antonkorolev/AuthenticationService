namespace BackendService.BusinessLogic.Operations.AuthenticateUser.Models;

public sealed class AuthenticateUserOperationResponse(string token)
{
    public string Token { get; } = token;
}