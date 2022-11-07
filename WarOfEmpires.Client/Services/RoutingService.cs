using System;

namespace WarOfEmpires.Client.Services;

public class RoutingService {
    public string GetRoute(Enum route) {
        var routeType = route.GetType();

        return $"{routeType.Name}/{Enum.GetName(routeType, route)}";
    }
}
