using System;
using System.Net.Http;

namespace WarOfEmpires.Client.Services;

public interface IAuthenticatedHttpClientProvider : IDisposable {
    HttpClient Provide();
}
