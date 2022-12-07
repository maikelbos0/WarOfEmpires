using System.Net.Http;
using System.Threading.Tasks;

namespace WarOfEmpires.Client.Services;

public interface IHttpClientFactory {
    Task<HttpClient> ProvideClient(bool requireAuthentication);

    HttpClient ProvideAnonymousClient();

    Task<HttpClient> ProvideAuthenticatedClient();
}
