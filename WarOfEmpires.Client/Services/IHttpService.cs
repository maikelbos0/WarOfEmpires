using System;
using System.Threading.Tasks;

namespace WarOfEmpires.Client.Services;

public interface IHttpService {
    public Task<TResponse?> PostAsync<TResponse, TRequest>(Enum route, TRequest request);
}
