using System.Net.Http;
using System.Threading.Tasks;

namespace WarOfEmpires.Client.Services;

public sealed class HttpClientProvider : IHttpClientProvider {
    private readonly IAuthenticatedHttpClientProvider authenticatedHttpClientProvider;
    private readonly IAnonymousHttpClientProvider anonymousHttpClientProvider;

    public HttpClientProvider(IAuthenticatedHttpClientProvider authenticatedHttpClientProvider, IAnonymousHttpClientProvider anonymousHttpClientProvider) {
        this.authenticatedHttpClientProvider = authenticatedHttpClientProvider;
        this.anonymousHttpClientProvider = anonymousHttpClientProvider;
    }

    public async Task<HttpClient> Provide(bool requireAuthentication) {
        if (requireAuthentication) {
            return await authenticatedHttpClientProvider.Provide();
        }
        else {
            return anonymousHttpClientProvider.Provide();
        }
    }
}
