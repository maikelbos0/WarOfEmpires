using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WarOfEmpires.Client.Services;

public interface IHttpClientProvider : IDisposable {
    Task<HttpClient> Provide(bool requireAuthentication);

    HttpClient ProvideAnonymousClient();

    Task<HttpClient> ProvideAuthenticatedClient();
}
