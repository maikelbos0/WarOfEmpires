using System;
using System.Net.Http;
using WarOfEmpires.Client.Configuration;

namespace WarOfEmpires.Client.Services;

public sealed class AnonymousHttpClientProvider : IAnonymousHttpClientProvider {
    private readonly ApiSettings apiSettings;
    private HttpClient? client;

    public AnonymousHttpClientProvider(ApiSettings apiSettings) {
        this.apiSettings = apiSettings;
    }

    public HttpClient Provide() => client ??= new HttpClient() {
        BaseAddress = new Uri(apiSettings.BaseUrl)
    };

    public void Dispose() {
        client?.Dispose();
    }
}
