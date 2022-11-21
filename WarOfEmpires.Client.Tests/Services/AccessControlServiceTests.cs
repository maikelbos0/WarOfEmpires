using Blazored.LocalStorage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using System;
using WarOfEmpires.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using FluentAssertions;
using WarOfEmpires.Api.Routing;

namespace WarOfEmpires.Client.Tests.Services;

[TestClass]
public class AccessControlServiceTests {
    private const string validToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ0ZXN0QHRlc3QuY29tIiwibmFtZSI6IlRlc3QiLCJyb2xlIjoiQWRtaW5pc3RyYXRvciIsIm5iZiI6MTY2ODg3MzcxNSwiZXhwIjo0ODIyNDczNzE1LCJpYXQiOjE2Njg4NzM3MTV9.6fJd0T_O_5sTtRCrJ4ZTurdq8mpin0wUZ4fSdEAUkdM";
    private const string expiredToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ0ZXN0QHRlc3QuY29tIiwibmFtZSI6IlRlc3QiLCJyb2xlIjoiQWRtaW5pc3RyYXRvciIsIm5iZiI6MTY2ODc2Njc2NywiZXhwIjoxNjY4NzcwMzY3LCJpYXQiOjE2Njg3NjY3Njd9.mxsVQsEVGCXuXJa7hafBzUkLExFGhqCH_nPNhNebBOc";

    [TestMethod]
    public async Task SignIn() {
        var storageService = Substitute.For<ILocalStorageService>();
        var service = new AccessControlService(storageService, Substitute.For<ITimerService>(), new JwtSecurityTokenHandler());
        AuthenticationState state = null;

        storageService.GetItemAsync<string>(Constants.AccessToken).Returns(validToken);
        service.AuthenticationStateChanged += async s => state = await s;

        await service.SignIn(validToken);

        await storageService.Received().SetItemAsync(Constants.AccessToken, validToken);
        state.User?.Identity.Should().NotBeNull();
        state.User.Identity.Name.Should().Be("Test");
        state.User.Identity.IsAuthenticated.Should().BeTrue();
        state.User.IsInRole(Roles.Administrator).Should().BeTrue();
    }

    [TestMethod]
    public async Task SignOut() {
        var storageService = Substitute.For<ILocalStorageService>();
        var service = new AccessControlService(storageService, Substitute.For<ITimerService>(), new JwtSecurityTokenHandler());
        AuthenticationState state = null;

        storageService.GetItemAsync<string>(Constants.AccessToken).Returns((string)null);
        service.AuthenticationStateChanged += async s => state = await s;

        await service.SignOut();

        await storageService.Received().RemoveItemAsync(Constants.AccessToken);
        state.User?.Identity.Should().NotBeNull();
        state.User.Identity.Name.Should().BeNull();
        state.User.Identity.IsAuthenticated.Should().BeFalse();
        state.User.IsInRole(Roles.Administrator).Should().BeFalse();
    }

    [TestMethod]
    public async Task GetAccessControlState_With_Valid_Token() {
        var storageService = Substitute.For<ILocalStorageService>();
        var timerService = Substitute.For<ITimerService>();
        var service = new AccessControlService(storageService, timerService, new JwtSecurityTokenHandler());

        storageService.GetItemAsync<string>(Constants.AccessToken).Returns(validToken);

        var state = await service.GetAuthenticationStateAsync();

        state.User?.Identity.Should().NotBeNull();
        state.User.Identity.Name.Should().Be("Test");
        state.User.Identity.IsAuthenticated.Should().BeTrue();
        state.User.IsInRole(Roles.Administrator).Should().BeTrue();
        timerService.Received().ExecuteAfter(service.SignOut, Arg.Is<TimeSpan>(timeSpan => timeSpan > (new DateTime(2122, 10, 26) - DateTime.UtcNow) && timeSpan < (new DateTime(2122, 10, 27) - DateTime.UtcNow)));
    }

    [TestMethod]
    public async Task GetAccessControlState_With_Expired_Token() {
        var storageService = Substitute.For<ILocalStorageService>();
        var timerService = Substitute.For<ITimerService>();
        var service = new AccessControlService(storageService, Substitute.For<ITimerService>(), new JwtSecurityTokenHandler());

        storageService.GetItemAsync<string>(Constants.AccessToken).Returns(expiredToken);

        var state = await service.GetAuthenticationStateAsync();

        state.User?.Identity.Should().NotBeNull();
        state.User.Identity.Name.Should().BeNull();
        state.User.Identity.IsAuthenticated.Should().BeFalse();
        state.User.IsInRole(Roles.Administrator).Should().BeFalse();
        timerService.DidNotReceiveWithAnyArgs().ExecuteAfter(default, default);
    }

    [TestMethod]
    public async Task GetAccessControlState_Without_Token() {
        var storageService = Substitute.For<ILocalStorageService>();
        var timerService = Substitute.For<ITimerService>();
        var service = new AccessControlService(storageService, Substitute.For<ITimerService>(), new JwtSecurityTokenHandler());

        storageService.GetItemAsync<string>(Constants.AccessToken).Returns((string)null);

        var state = await service.GetAuthenticationStateAsync();

        state.User?.Identity.Should().NotBeNull();
        state.User.Identity.Name.Should().BeNull();
        state.User.Identity.IsAuthenticated.Should().BeFalse();
        state.User.IsInRole(Roles.Administrator).Should().BeFalse();
        timerService.DidNotReceiveWithAnyArgs().ExecuteAfter(default, default);
    }
}
