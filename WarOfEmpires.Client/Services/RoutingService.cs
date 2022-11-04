using System;

namespace WarOfEmpires.Client.Services;

public class RoutingService {
    public string GetRoute<TRoute>(TRoute route) where TRoute : struct, Enum
        => $"{typeof(TRoute).Name}/{Enum.GetName(route)}";
}
