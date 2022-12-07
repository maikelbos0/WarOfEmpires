using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Client.Configuration;
using WarOfEmpires.Client.Services;

namespace WarOfEmpires.Client.Tests.Services;

[TestClass]
public class AnonymousHttpClientProviderTests {
    [TestMethod]
    public void AnonymousHttpClientProvider_Provide_Returns_Configured_Client() {
        var provider = new AnonymousHttpClientProvider(new ApiSettings() { BaseUrl = "https://localhost:1234/" });

        var client = provider.Provide();

        client.BaseAddress.ToString().Should().Be("https://localhost:1234/");
    }

    [TestMethod]
    public void AnonymousHttpClientProvider_Caches_Client() {
        var provider = new AnonymousHttpClientProvider(new ApiSettings() { BaseUrl = "https://localhost:1234/" });

        var client = provider.Provide();

        client.Should().BeSameAs(provider.Provide());
    }
}
