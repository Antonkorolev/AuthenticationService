using BackendService.AuthorizationService.Contracts.Request;
using BackendService.AuthorizationService.Contracts.Response;

namespace BackendService.AuthorizationService.Contracts;

public interface IAuthorizationServiceClient
{
    Task<GetPermissionsResponse> GetPermissionsAsync(GetPermissionsRequest request);

    Task AddPermissionsToUserAsync(AddPermissionsToUserRequest request);

    Task DeletePermissionsFromUserAsync(DeletePermissionsFromUserRequest request);
}