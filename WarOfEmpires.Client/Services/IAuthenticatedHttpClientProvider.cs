using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WarOfEmpires.Client.Services;

public interface IAuthenticatedHttpClientProvider : IDisposable {
    Task<HttpClient> Provide();
}
