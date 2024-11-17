using BackendService.BusinessLogic.Operations.AuthenticateUser.Models;

namespace BackendService.BusinessLogic.Operations.AuthenticateUser;

public interface IAuthenticateUserOperation
{
    Task<AuthenticateUserOperationResponse> AuthenticateAsync(AuthenticateUserOperationRequest request);
}