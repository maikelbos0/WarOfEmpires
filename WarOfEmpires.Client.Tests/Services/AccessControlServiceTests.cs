using Blazored.LocalStorage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using WarOfEmpires.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using FluentAssertions;
using WarOfEmpires.Api.Routing;
using WarOfEmpires.Models.Security;

namespace WarOfEmpires.Client.Tests.Services;

[TestClass]
public class AccessControlServiceTests {
    private const string validToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ0ZXN0QHRlc3QuY29tIiwibmFtZSI6IlRlc3QiLCJyb2xlcyI6IkFkbWluaXN0cmF0b3IiLCJuYmYiOjE2Njg4NzM3MTUsImV4cCI6NDgyMjQ3MzcxNSwiaWF0IjoxNjY4ODczNzE1fQ.fS3OaNVZqbM4Wgm9qT6MrMjPz9jwD52rMUsclvUdJ3k";
    private const string expiredToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ0ZXN0QHRlc3QuY29tIiwibmFtZSI6IlRlc3QiLCJyb2xlIjoiQWRtaW5pc3RyYXRvciIsIm5iZiI6MTY2ODc2Njc2NywiZXhwIjoxNjY4NzcwMzY3LCJpYXQiOjE2Njg3NjY3Njd9.mxsVQsEVGCXuXJa7hafBzUkLExFGhqCH_nPNhNebBOc";
    private const string refreshToken = "refresh-token";

    [TestMethod]
    public async Task AccessControlService_SignIn_Succeeds() {
        var storageService = Substitute.For<ILocalStorageService>();
        var service = new AccessControlService(storageService, new JwtSecurityTokenHandler(), Substitute.For<ITokenService>());
        var tokens = new UserTokenModel() { AccessToken = validToken, RefreshToken = refreshToken };
        AuthenticationState notifiedState = null;

        storageService.GetItemAsync<string>(Constants.Tokens).Returns(validToken);
        service.AuthenticationStateChanged += async s => notifiedState = await s;

        await service.SignIn(tokens);

        await storageService.Received().SetItemAsync(Constants.Tokens, tokens);
        notifiedState?.User?.Identity.Should().NotBeNull();
        notifiedState.User.Identity.Name.Should().Be("Test");
        notifiedState.User.Identity.IsAuthenticated.Should().BeTrue();
        notifiedState.User.IsInRole(Roles.Administrator).Should().BeTrue();
    }

    [TestMethod]
    public async Task AccessControlService_SignOut_Succeeds() {
        var storageService = Substitute.For<ILocalStorageService>();
        var service = new AccessControlService(storageService, new JwtSecurityTokenHandler(), Substitute.For<ITokenService>());
        AuthenticationState notifiedState = null;

        storageService.GetItemAsync<string>(Constants.Tokens).Returns((string)null);
        service.AuthenticationStateChanged += async s => notifiedState = await s;

        await service.SignOut();

        await storageService.Received().RemoveItemAsync(Constants.Tokens);
        notifiedState?.User?.Identity.Should().NotBeNull();
        notifiedState.User.Identity.Name.Should().BeNull();
        notifiedState.User.Identity.IsAuthenticated.Should().BeFalse();
        notifiedState.User.IsInRole(Roles.Administrator).Should().BeFalse();
    }

    [TestMethod]
    public async Task AccessControlService_GetAccessControlState_With_Valid_Token_Succeeds() {
        var storageService = Substitute.For<ILocalStorageService>();
        var service = new AccessControlService(storageService, new JwtSecurityTokenHandler(), Substitute.For<ITokenService>());
        var tokens = new UserTokenModel() { AccessToken = validToken, RefreshToken = refreshToken };

        storageService.GetItemAsync<UserTokenModel>(Constants.Tokens).Returns(tokens);

        var state = await service.GetAuthenticationStateAsync();

        state?.User?.Identity.Should().NotBeNull();
        state.User.Identity.Name.Should().Be("Test");
        state.User.Identity.IsAuthenticated.Should().BeTrue();
        state.User.IsInRole(Roles.Administrator).Should().BeTrue();
    }

    [TestMethod]
    public async Task AccessControlService_GetAccessControlState_With_Expired_Token_Succeeds() {
        var storageService = Substitute.For<ILocalStorageService>();
        var tokenService = Substitute.For<ITokenService>();
        var service = new AccessControlService(storageService, new JwtSecurityTokenHandler(), tokenService);
        var tokens = new UserTokenModel() { AccessToken = expiredToken, RefreshToken = refreshToken };
        var newTokens = new UserTokenModel() { AccessToken = validToken, RefreshToken = refreshToken };

        tokenService.AcquireNewTokens(tokens).Returns(newTokens);
        storageService.GetItemAsync<UserTokenModel>(Constants.Tokens).Returns(tokens);

        var state = await service.GetAuthenticationStateAsync();

        state?.User.Identity.Name.Should().Be("Test");
        state.User.Identity.IsAuthenticated.Should().BeTrue();
        state.User.IsInRole(Roles.Administrator).Should().BeTrue();
        await storageService.Received().SetItemAsync(Constants.Tokens, newTokens);
    }

    [TestMethod]
    public async Task AccessControlService_GetAccessControlState_With_Expired_Token_And_No_New_Token_Succeeds() {
        var storageService = Substitute.For<ILocalStorageService>();
        var tokenService = Substitute.For<ITokenService>();
        var service = new AccessControlService(storageService, new JwtSecurityTokenHandler(), tokenService);
        var tokens = new UserTokenModel() { AccessToken = expiredToken, RefreshToken = refreshToken };
        AuthenticationState notifiedState = null;

        tokenService.AcquireNewTokens(tokens).Returns((UserTokenModel)null);
        storageService.GetItemAsync<UserTokenModel>(Constants.Tokens).Returns(tokens);
        service.AuthenticationStateChanged += async s => notifiedState = await s;

        var state = await service.GetAuthenticationStateAsync();

        state?.User?.Identity.Should().NotBeNull();
        state.User.Identity.Name.Should().BeNull();
        state.User.Identity.IsAuthenticated.Should().BeFalse();
        state.User.IsInRole(Roles.Administrator).Should().BeFalse();

        notifiedState?.User?.Identity.Should().NotBeNull();
        notifiedState.User.Identity.Name.Should().BeNull();
        notifiedState.User.Identity.IsAuthenticated.Should().BeFalse();
        notifiedState.User.IsInRole(Roles.Administrator).Should().BeFalse();
        await storageService.Received().RemoveItemAsync(Constants.Tokens);
    }

    [TestMethod]
    public async Task AccessControlService_GetAccessControlState_Without_Token_Succeeds() {
        var storageService = Substitute.For<ILocalStorageService>();
        var service = new AccessControlService(storageService, new JwtSecurityTokenHandler(), Substitute.For<ITokenService>());

        storageService.GetItemAsync<UserTokenModel>(Constants.Tokens).Returns((UserTokenModel)null);

        var state = await service.GetAuthenticationStateAsync();

        state?.User?.Identity.Should().NotBeNull();
        state.User.Identity.Name.Should().BeNull();
        state.User.Identity.IsAuthenticated.Should().BeFalse();
        state.User.IsInRole(Roles.Administrator).Should().BeFalse();
    }
}
