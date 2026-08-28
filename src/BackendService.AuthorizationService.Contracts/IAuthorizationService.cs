using BackendService.AuthorizationService.Contracts.Request;
using BackendService.AuthorizationService.Contracts.Response;

namespace BackendService.AuthorizationService.Contracts;

public interface IAuthorizationService
{
    Task<GetPermissionsResponse> GetPermissionsAsync(GetPermissionsRequest request);

    Task AddPermissionsToUserAsync(AddPermissionsToUserRequest request);

    Task DeletePermissionsFromUserAsync(DeletePermissionsFromUserRequest request);
}