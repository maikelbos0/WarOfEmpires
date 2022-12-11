using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace WarOfEmpires.Client.Services;

public class HttpMessageAuthenticationHandler : DelegatingHandler {
    private readonly IAccessControlService accessControlService;

    public HttpMessageAuthenticationHandler(IAccessControlService accessControlService, HttpMessageHandler innerHandler) {
        this.accessControlService = accessControlService;
        InnerHandler = innerHandler;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
        var token = await accessControlService.GetAccessToken();

        if (token != null) {
            request.Headers.Authorization = new AuthenticationHeaderValue(Constants.AuthenticationScheme, token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
