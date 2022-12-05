using Blazored.LocalStorage;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace WarOfEmpires.Client.Services;

public class HttpMessageAuthorizationHandler : DelegatingHandler {
    private readonly ILocalStorageService storageService;

    public HttpMessageAuthorizationHandler(ILocalStorageService storageService, HttpMessageHandler innerHandler) {
        this.storageService = storageService;
        InnerHandler = innerHandler;
    }

    // TODO fix
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
        var token = await storageService.GetItemAsync<string?>(Constants.Tokens, cancellationToken);

        if (token != null) {
            request.Headers.Authorization = new AuthenticationHeaderValue(Constants.AuthenticationScheme, token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
