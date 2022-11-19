using Blazored.LocalStorage;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using WarOfEmpires.Client.Services;

namespace WarOfEmpires.Client.Tests.Services;

[TestClass]
public class AccessControlProviderTests {
    private const string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ0ZXN0QHRlc3QuY29tIiwibmFtZSI6IlRlc3QiLCJyb2xlIjoiQWRtaW5pc3RyYXRvciIsIm5iZiI6MTY2ODc2Njc2NywiZXhwIjoxNjY4NzcwMzY3LCJpYXQiOjE2Njg3NjY3Njd9.mxsVQsEVGCXuXJa7hafBzUkLExFGhqCH_nPNhNebBOc";

    [TestMethod]
    public async Task SignIn() {
        var storageService = Substitute.For<ILocalStorageService>();
        var provider = new AccessControlProvider(storageService, new JwtSecurityTokenHandler());
        AccessControlState state = null;

        provider.AccessControlStateChanged += s => state = s;

        await provider.SignIn(token);

        await storageService.Received().SetItemAsync(Constants.AccessToken, token);
        state.Should().NotBeNull();
        state.DisplayName.Should().Be("Test");
        state.IsAuthenticated.Should().BeTrue();
        state.IsAdmin.Should().BeTrue();
    }

    [TestMethod]
    public async Task SignOut() {
        var storageService = Substitute.For<ILocalStorageService>();
        var provider = new AccessControlProvider(storageService, new JwtSecurityTokenHandler());
        AccessControlState state = null;

        provider.AccessControlStateChanged += s => state = s;

        await provider.SignOut();

        await storageService.Received().RemoveItemAsync(Constants.AccessToken);
        state.Should().NotBeNull();
        state.DisplayName.Should().BeNull();
        state.IsAuthenticated.Should().BeFalse();
        state.IsAdmin.Should().BeFalse();
    }

    [TestMethod]
    public async Task GetAccessControlState_With_Token() {
        var storageService = Substitute.For<ILocalStorageService>();
        var provider = new AccessControlProvider(storageService, new JwtSecurityTokenHandler());

        storageService.GetItemAsync<string>(Constants.AccessToken).Returns(token);

        var state = await provider.GetAccessControlState();

        state.Should().NotBeNull();
        state.DisplayName.Should().Be("Test");
        state.IsAuthenticated.Should().BeTrue();
        state.IsAdmin.Should().BeTrue();
    }

    [TestMethod]
    public async Task GetAccessControlState_Without_Token() {
        var storageService = Substitute.For<ILocalStorageService>();
        var provider = new AccessControlProvider(storageService, new JwtSecurityTokenHandler());

        storageService.GetItemAsync<string>(Constants.AccessToken).Returns((string)null);

        var state = await provider.GetAccessControlState();
        
        state.Should().NotBeNull();
        state.DisplayName.Should().BeNull();
        state.IsAuthenticated.Should().BeFalse();
        state.IsAdmin.Should().BeFalse();
    }
}
