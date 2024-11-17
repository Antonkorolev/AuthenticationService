using BackendService.Contracts.AuthenticateUser;

namespace BackendService.Contracts;

public interface IAuthenticateService
{
    Task<AuthenticateUserResponse> AuthenticateUserAsync(AuthenticateUserRequest request);
}