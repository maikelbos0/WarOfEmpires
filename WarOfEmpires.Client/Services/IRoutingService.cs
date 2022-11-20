using System;

namespace WarOfEmpires.Client.Services;

public interface IRoutingService {
    string GetRoute(Enum route);
}