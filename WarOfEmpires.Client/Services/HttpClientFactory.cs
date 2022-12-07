using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WarOfEmpires.Client.Configuration;

namespace WarOfEmpires.Client.Services;

public sealed class HttpClientFactory : IHttpClientFactory, IDisposable {
    private readonly IAccessControlService accessControlService;
    private readonly ApiSettings apiSettings;
    private HttpClient? anonymousClient;
    private HttpClient? authenticatedClient;

    public HttpClientFactory(IAccessControlService accessControlService, ApiSettings apiSettings) {
        this.accessControlService = accessControlService;
        this.apiSettings = apiSettings;
    }

    // todo test
    public async Task<HttpClient> ProvideClient(bool requireAuthentication) {
        if (requireAuthentication) {
            return await ProvideAuthenticatedClient();
        }
        else {
            return ProvideAnonymousClient();
        }
    }

    public HttpClient ProvideAnonymousClient() {
        if (anonymousClient == null) {
            anonymousClient = new HttpClient() {
                BaseAddress = new Uri(apiSettings.BaseUrl)
            };
        }

        return anonymousClient;
    }

    public async Task<HttpClient> ProvideAuthenticatedClient() {
        if (authenticatedClient == null) {
            authenticatedClient = new HttpClient() {
                BaseAddress = new Uri(apiSettings.BaseUrl),
                DefaultRequestHeaders = {
                    Authorization = new AuthenticationHeaderValue(Constants.AuthenticationScheme, await accessControlService.GetAccessToken())
                }
            };
        }

        return authenticatedClient;
    }

    public void Dispose() {
        anonymousClient?.Dispose();
        authenticatedClient?.Dispose();
    }
}
