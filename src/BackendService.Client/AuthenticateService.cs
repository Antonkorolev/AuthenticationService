using System.Net.Http.Json;
using System.Text.Json;
using BackendService.Contracts;
using BackendService.Contracts.AuthenticateUser;

namespace BackendService.Client;

public class AuthenticateService : IAuthenticateService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _httpClientName;

    public AuthenticateService(IHttpClientFactory httpClientFactory, string httpClientName)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentException("HttpClientFactory not configured");
        _httpClientName = httpClientName;
    }

    public async Task<AuthenticateUserResponse> AuthenticateUserAsync(AuthenticateUserRequest request)
    {
        var httpClient = CreateHttpClient();

        var httpResponseMessage = await httpClient.PostAsync($"user/Authenticate", JsonContent.Create(request)).ConfigureAwait(false);

        if (!httpResponseMessage.IsSuccessStatusCode)
            throw new Exception($"Response StatusCode: {httpResponseMessage.StatusCode}, Content: {httpResponseMessage.Content}");

        var responseString = await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);
        var response = JsonSerializer.Deserialize<AuthenticateUserResponse>(responseString);

        if (response == null)
            throw new FormatException("Can't deserialize HttpResponseMessage to object");

        return response;
    }

    private HttpClient CreateHttpClient()
    {
        return _httpClientFactory.CreateClient(_httpClientName);
    }
}