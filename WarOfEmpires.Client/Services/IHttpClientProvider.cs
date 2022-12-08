using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WarOfEmpires.Client.Services;

public interface IHttpClientProvider {
    Task<HttpClient> Provide(bool requireAuthentication);
}
