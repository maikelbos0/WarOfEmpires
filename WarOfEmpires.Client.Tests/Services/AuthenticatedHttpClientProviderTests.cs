using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Threading.Tasks;
using WarOfEmpires.Client.Configuration;
using WarOfEmpires.Client.Services;

namespace WarOfEmpires.Client.Tests.Services;

[TestClass]
public class AuthenticatedHttpClientProviderTests {
    [TestMethod]
    public async Task AuthenticatedHttpClientProvider_Provide_Returns_Configured_Client() {
        var accessControlService = Substitute.For<IAccessControlService>();
        var provider = new AuthenticatedHttpClientProvider(accessControlService, new ApiSettings() { BaseUrl = "https://localhost:1234/" });

        accessControlService.GetAccessToken().Returns("token");

        var client = await provider.Provide();

        client.BaseAddress.ToString().Should().Be("https://localhost:1234/");
        client.DefaultRequestHeaders.Authorization.Should().NotBeNull();
        client.DefaultRequestHeaders.Authorization.Scheme.Should().Be(Constants.AuthenticationScheme);
        client.DefaultRequestHeaders.Authorization.Parameter.Should().Be("token");

    }

    [TestMethod]
    public async Task AuthenticatedHttpClientProvider_Caches_Client() {
        var provider = new AuthenticatedHttpClientProvider(Substitute.For<IAccessControlService>(), new ApiSettings() { BaseUrl = "https://localhost:1234/" });

        var client = await provider.Provide();

        client.Should().BeSameAs(await provider.Provide());
    }
}
