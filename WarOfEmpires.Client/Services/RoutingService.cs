﻿using System;

namespace WarOfEmpires.Client.Services;

public sealed class RoutingService : IRoutingService {
    public string GetRoute(Enum route) {
        var routeType = route.GetType();

        return $"{routeType.Name}/{Enum.GetName(routeType, route)}";
    }
}
