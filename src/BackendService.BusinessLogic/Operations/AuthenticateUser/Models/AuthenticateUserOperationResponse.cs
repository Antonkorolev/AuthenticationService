namespace BackendService.BusinessLogic.Operations.AuthenticateUser.Models;

public sealed class AuthenticateUserOperationResponse(bool isAuthenticated)
{
    public bool IsAuthenticated { get; set; } = isAuthenticated;
}