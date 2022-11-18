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
    [TestMethod]
    public async Task SignIn() {
        var storageService = Substitute.For<ILocalStorageService>();
        var provider = new AccessControlProvider(storageService);
        AccessControlState state = null;

        provider.AccessControlStateChanged += s => state = s;

        await provider.SignIn("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ0ZXN0QHRlc3QuY29tIiwibmFtZSI6IlRlc3QiLCJyb2xlIjoiQWRtaW5pc3RyYXRvciIsIm5iZiI6MTY2ODc2Njc2NywiZXhwIjoxNjY4NzcwMzY3LCJpYXQiOjE2Njg3NjY3Njd9.mxsVQsEVGCXuXJa7hafBzUkLExFGhqCH_nPNhNebBOc");

        await storageService.Received().SetItemAsync("AccessToken", Arg.Any<JwtSecurityToken>());
        state.Should().NotBeNull();
        state.DisplayName.Should().Be("Test");
        state.IsAuthenticated.Should().BeTrue();
        state.IsAdmin.Should().BeTrue();
    }

    [TestMethod]
    public async Task SignOut() {
        var storageService = Substitute.For<ILocalStorageService>();
        var provider = new AccessControlProvider(storageService);
        AccessControlState state = null;

        provider.AccessControlStateChanged += s => state = s;

        await provider.SignOut();

        await storageService.Received().RemoveItemAsync("AccessToken");
        state.Should().NotBeNull();
        state.DisplayName.Should().BeNull();
        state.IsAuthenticated.Should().BeFalse();
        state.IsAdmin.Should().BeFalse();
    }
}
