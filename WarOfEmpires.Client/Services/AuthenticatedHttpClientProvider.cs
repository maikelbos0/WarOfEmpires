using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WarOfEmpires.Client.Configuration;

namespace WarOfEmpires.Client.Services;

public sealed class AuthenticatedHttpClientProvider : IAuthenticatedHttpClientProvider {
    private readonly IAccessControlService accessControlService;
    private readonly ApiSettings apiSettings;
    private HttpClient? client;

    public AuthenticatedHttpClientProvider(IAccessControlService accessControlService, ApiSettings apiSettings) {
        this.accessControlService = accessControlService;
        this.apiSettings = apiSettings;
    }

    public async Task<HttpClient> Provide() => client ??= new HttpClient() {
        BaseAddress = new Uri(apiSettings.BaseUrl),
        DefaultRequestHeaders = {
            // TODO figure out what happens if this token expires
            Authorization = new AuthenticationHeaderValue(Constants.AuthenticationScheme, await accessControlService.GetAccessToken())
        }
    };

    public void Dispose() {
        client?.Dispose();
    }
}
