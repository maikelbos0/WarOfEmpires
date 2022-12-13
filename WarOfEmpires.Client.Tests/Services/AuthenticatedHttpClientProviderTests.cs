using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Net.Http;
using WarOfEmpires.Client.Configuration;
using WarOfEmpires.Client.Services;

namespace WarOfEmpires.Client.Tests.Services;

[TestClass]
public class AuthenticatedHttpClientProviderTests {
    [TestMethod]
    public void AuthenticatedHttpClientProvider_Provide_Returns_Configured_Client() {
        var provider = new AuthenticatedHttpClientProvider(Substitute.For<HttpMessageAuthenticationHandler>(Substitute.For<IAccessControlService>(), Substitute.For<HttpMessageHandler>()), new ApiSettings() { BaseUrl = "https://localhost:1234/" });

        var client = provider.Provide();

        client.BaseAddress.ToString().Should().Be("https://localhost:1234/");
    }

    [TestMethod]
    public void AuthenticatedHttpClientProvider_Caches_Client() {
        var provider = new AuthenticatedHttpClientProvider(Substitute.For<HttpMessageAuthenticationHandler>(Substitute.For<IAccessControlService>(), Substitute.For<HttpMessageHandler>()), new ApiSettings() { BaseUrl = "https://localhost:1234/" });

        var client = provider.Provide();

        client.Should().BeSameAs(provider.Provide());
    }
}
