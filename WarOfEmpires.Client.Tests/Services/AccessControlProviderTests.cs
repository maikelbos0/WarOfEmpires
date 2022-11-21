using Blazored.LocalStorage;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using WarOfEmpires.Client.Services;

namespace WarOfEmpires.Client.Tests.Services;

[TestClass]
public class AccessControlProviderTests {
    private const string validToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ0ZXN0QHRlc3QuY29tIiwibmFtZSI6IlRlc3QiLCJyb2xlIjoiQWRtaW5pc3RyYXRvciIsIm5iZiI6MTY2ODg3MzcxNSwiZXhwIjo0ODIyNDczNzE1LCJpYXQiOjE2Njg4NzM3MTV9.6fJd0T_O_5sTtRCrJ4ZTurdq8mpin0wUZ4fSdEAUkdM";
    private const string expiredToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ0ZXN0QHRlc3QuY29tIiwibmFtZSI6IlRlc3QiLCJyb2xlIjoiQWRtaW5pc3RyYXRvciIsIm5iZiI6MTY2ODc2Njc2NywiZXhwIjoxNjY4NzcwMzY3LCJpYXQiOjE2Njg3NjY3Njd9.mxsVQsEVGCXuXJa7hafBzUkLExFGhqCH_nPNhNebBOc";

    [TestMethod]
    public async Task SignIn() {
        var storageService = Substitute.For<ILocalStorageService>();
        var provider = new AccessControlProvider(storageService, Substitute.For<ITimerService>(), new JwtSecurityTokenHandler());
        AccessControlState state = null;

        storageService.GetItemAsync<string>(Constants.AccessToken).Returns(validToken);
        provider.AccessControlStateChanged += s => { state = s; return Task.CompletedTask; };

        await provider.SignIn(validToken);

        await storageService.Received().SetItemAsync(Constants.AccessToken, validToken);
        state.Should().NotBeNull();
        state.DisplayName.Should().Be("Test");
        state.IsAuthenticated.Should().BeTrue();
        state.IsAdmin.Should().BeTrue();
    }

    [TestMethod]
    public async Task SignOut() {
        var storageService = Substitute.For<ILocalStorageService>();
        var provider = new AccessControlProvider(storageService, Substitute.For<ITimerService>(), new JwtSecurityTokenHandler());
        AccessControlState state = null;

        storageService.GetItemAsync<string>(Constants.AccessToken).Returns((string)null);
        provider.AccessControlStateChanged += s => { state = s; return Task.CompletedTask; };

        await provider.SignOut();

        await storageService.Received().RemoveItemAsync(Constants.AccessToken);
        state.Should().NotBeNull();
        state.DisplayName.Should().BeNull();
        state.IsAuthenticated.Should().BeFalse();
        state.IsAdmin.Should().BeFalse();
    }

    [TestMethod]
    public async Task GetAccessControlState_With_Valid_Token() {
        var storageService = Substitute.For<ILocalStorageService>();
        var timerService = Substitute.For<ITimerService>();
        var provider = new AccessControlProvider(storageService, timerService, new JwtSecurityTokenHandler());

        storageService.GetItemAsync<string>(Constants.AccessToken).Returns(validToken);

        var state = await provider.GetAccessControlState();

        state.Should().NotBeNull();
        state.DisplayName.Should().Be("Test");
        state.IsAuthenticated.Should().BeTrue();
        state.IsAdmin.Should().BeTrue();

        timerService.Received().ExecuteAfter(provider.SignOut, Arg.Is<TimeSpan>(timeSpan => timeSpan > (new DateTime(2122, 10, 26) - DateTime.UtcNow) && timeSpan < (new DateTime(2122, 10, 27) - DateTime.UtcNow)));
    }

    [TestMethod]
    public async Task GetAccessControlState_With_Expired_Token() {
        var storageService = Substitute.For<ILocalStorageService>();
        var timerService = Substitute.For<ITimerService>();
        var provider = new AccessControlProvider(storageService, timerService, new JwtSecurityTokenHandler());

        storageService.GetItemAsync<string>(Constants.AccessToken).Returns(expiredToken);

        var state = await provider.GetAccessControlState();
        state.Should().NotBeNull();
        state.DisplayName.Should().BeNull();
        state.IsAuthenticated.Should().BeFalse();
        state.IsAdmin.Should().BeFalse();

        timerService.DidNotReceiveWithAnyArgs().ExecuteAfter(default, default);
    }

    [TestMethod]
    public async Task GetAccessControlState_Without_Token() {
        var storageService = Substitute.For<ILocalStorageService>();
        var timerService = Substitute.For<ITimerService>();
        var provider = new AccessControlProvider(storageService, timerService, new JwtSecurityTokenHandler());

        storageService.GetItemAsync<string>(Constants.AccessToken).Returns((string)null);

        var state = await provider.GetAccessControlState();
        state.Should().NotBeNull();
        state.DisplayName.Should().BeNull();
        state.IsAuthenticated.Should().BeFalse();
        state.IsAdmin.Should().BeFalse();

        timerService.DidNotReceiveWithAnyArgs().ExecuteAfter(default, default);
    }
}
