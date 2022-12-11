using System;
using System.Net.Http;
using WarOfEmpires.Client.Configuration;

namespace WarOfEmpires.Client.Services;

public sealed class AuthenticatedHttpClientProvider : IAuthenticatedHttpClientProvider {
    private readonly HttpMessageAuthenticationHandler httpMessageAuthenticationHandler;
    private readonly ApiSettings apiSettings;
    private HttpClient? client;

    public AuthenticatedHttpClientProvider(HttpMessageAuthenticationHandler httpMessageAuthenticationHandler, ApiSettings apiSettings) {
        this.httpMessageAuthenticationHandler = httpMessageAuthenticationHandler;
        this.apiSettings = apiSettings;
    }

    public HttpClient Provide() => client ??= new HttpClient(httpMessageAuthenticationHandler) {
        BaseAddress = new Uri(apiSettings.BaseUrl)
    };

    public void Dispose() {
        client?.Dispose();
    }
}
