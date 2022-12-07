using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WarOfEmpires.Api.Routing;
using WarOfEmpires.Models.Security;

namespace WarOfEmpires.Client.Services;

public sealed class TokenService : ITokenService {
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IRoutingService routingService;

    public TokenService(IHttpClientFactory httpClientFactory, IRoutingService routingService) {
        this.httpClientFactory = httpClientFactory;
        this.routingService = routingService;
    }

    public async Task<UserTokenModel?> AcquireNewTokens(UserTokenModel tokens) {
        var httpClient = httpClientFactory.ProvideAnonymousClient();
        var response = await httpClient.PostAsJsonAsync(routingService.GetRoute(Security.AcquireNewTokens), tokens);

        if (response.IsSuccessStatusCode) {
            return await response.Content.ReadFromJsonAsync<UserTokenModel>() ?? throw new InvalidOperationException("Missing tokens in response content");
        }

        return null;
    }
}
