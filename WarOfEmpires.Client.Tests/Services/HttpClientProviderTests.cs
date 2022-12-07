using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Net.Http;
using System.Threading.Tasks;
using WarOfEmpires.Client.Services;

namespace WarOfEmpires.Client.Tests.Services;

[TestClass]
public class HttpClientProviderTests {
    [TestMethod]
    public async Task HttpClientProvider_Provide_RequireAuthentication_True_Uses_AuthenticatedHttpClientProvider() {
        var authenticatedHttpClientProvider = Substitute.For<IAuthenticatedHttpClientProvider>();
        var authenticatedHttpClient = new HttpClient();
        var anonymousHttpClientProvider = Substitute.For<IAnonymousHttpClientProvider>();
        var provider = new HttpClientProvider(authenticatedHttpClientProvider, anonymousHttpClientProvider);

        authenticatedHttpClientProvider.Provide().Returns(authenticatedHttpClient);

        var httpClient = await provider.Provide(true);

        httpClient.Should().BeSameAs(authenticatedHttpClient);
        await authenticatedHttpClientProvider.Received().Provide();
        anonymousHttpClientProvider.DidNotReceive().Provide();
    }

    [TestMethod]
    public async Task HttpClientProvider_Provide_RequireAuthentication_False_Uses_AnonymousHttpClientProvider() {
        var authenticatedHttpClientProvider = Substitute.For<IAuthenticatedHttpClientProvider>();
        var anonymousHttpClientProvider = Substitute.For<IAnonymousHttpClientProvider>();
        var anonymousHttpClient = new HttpClient();
        var provider = new HttpClientProvider(authenticatedHttpClientProvider, anonymousHttpClientProvider);

        anonymousHttpClientProvider.Provide().Returns(anonymousHttpClient);

        var httpClient = await provider.Provide(false);

        httpClient.Should().BeSameAs(anonymousHttpClient);
        await authenticatedHttpClientProvider.DidNotReceive().Provide();
        anonymousHttpClientProvider.Received().Provide();
    }
}
