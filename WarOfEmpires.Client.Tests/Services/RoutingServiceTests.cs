using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Client.Services;

namespace WarOfEmpires.Client.Tests.Services;

[TestClass]
public sealed class RoutingServiceTests {
    private enum Test {
        Route
    }

    [TestMethod]
    public void RoutingService_GetRoute_Succeeds() {
        var service = new RoutingService();

        service.GetRoute(Test.Route).Should().Be("Test/Route");
    }
}
