using System;
using System.Net.Http;

namespace WarOfEmpires.Client.Services;

public interface IAnonymousHttpClientProvider : IDisposable {
    HttpClient Provide();
}
