using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace WarOfEmpires.Client.Services;

public sealed class HttpService : IHttpService {
    private readonly HttpClient httpClient;
    private readonly IRoutingService routingService;

    public HttpService(HttpClient httpClient, IRoutingService routingService) {
        this.httpClient = httpClient;
        this.routingService = routingService;
    }

    public async Task<TResponse?> PostAsync<TResponse, TRequest>(Enum route, TRequest request) {
        var response = await httpClient.PostAsJsonAsync(routingService.GetRoute(route), request);

        if (response.IsSuccessStatusCode) {
            return await response.Content.ReadFromJsonAsync<TResponse>() ?? throw new InvalidOperationException("Missing response content");
        }

        return default;
    }
}
