using System.Net.Http.Json;
using BackendService.AuthorizationService.Contracts;
using BackendService.AuthorizationService.Contracts.Request;
using BackendService.AuthorizationService.Contracts.Response;

namespace BackendService.AuthorizationService.Client;

public sealed class AuthorizationServiceClient(IHttpClientFactory httpClientFactory) : IAuthorizationService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(nameof(AuthorizationServiceClient));

    public async Task<GetPermissionsResponse> GetPermissionsAsync(GetPermissionsRequest request)
    {
        var response = await _httpClient.PostAsync("permission/getPermissions", JsonContent.Create(request));

        response.EnsureSuccessStatusCode();

        var getPermissionsResponse = await response.Content.ReadFromJsonAsync<GetPermissionsResponse>();
        
        if(getPermissionsResponse != null)
            return getPermissionsResponse;

        throw new Exception("Can not deserialize to GetPermissionsResponse");
    }

    public async Task AddPermissionsToUserAsync(AddPermissionsToUserRequest request)
    {
        var response = await _httpClient.PostAsync("permission/addPermissionsToUser", JsonContent.Create(request));

        response.EnsureSuccessStatusCode();
    }

    public async Task DeletePermissionsFromUserAsync(DeletePermissionsFromUserRequest request)
    {
        var response = await _httpClient.PostAsync("permission/deletePermissionsFromUser", JsonContent.Create(request));
        response.EnsureSuccessStatusCode();
    }
}