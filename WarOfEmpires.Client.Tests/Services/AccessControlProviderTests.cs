using Blazored.LocalStorage;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

    [TestMethod]
    public async Task IsAuthenticated_Is_True_With_Token() {
        var storageService = Substitute.For<ILocalStorageService>();
        var provider = new AccessControlProvider(storageService);
        
        storageService.GetItemAsync<JwtSecurityToken>("AccessToken").Returns(new JwtSecurityToken());

        (await provider.IsAuthenticated()).Should().BeTrue();
    }

    [TestMethod]
    public async Task IsAuthenticated_Is_False_Without_Token() {
        var storageService = Substitute.For<ILocalStorageService>();
        var provider = new AccessControlProvider(storageService);

        storageService.GetItemAsync<JwtSecurityToken>("AccessToken").Returns((JwtSecurityToken)null);

        (await provider.IsAuthenticated()).Should().BeFalse();
    }

    [TestMethod]
    public async Task GetName_Returns_Name_Claim_From_Token() {
        var storageService = Substitute.For<ILocalStorageService>();
        var provider = new AccessControlProvider(storageService);

        storageService.GetItemAsync<JwtSecurityToken>("AccessToken").Returns(new JwtSecurityToken(claims: new List<Claim>() {
            new Claim(JwtRegisteredClaimNames.Name, "Test")
        }));

        (await provider.GetName()).Should().Be("Test");
    }

    [TestMethod]
    public async Task GetName_Returns_Null_Without_Name_Claim() {
        var storageService = Substitute.For<ILocalStorageService>();
        var provider = new AccessControlProvider(storageService);

        storageService.GetItemAsync<JwtSecurityToken>("AccessToken").Returns(new JwtSecurityToken());

        (await provider.GetName()).Should().BeNull();
    }

    [TestMethod]
    public async Task GetName_Returns_Null_Without_Token() {
        var storageService = Substitute.For<ILocalStorageService>();
        var provider = new AccessControlProvider(storageService);

        storageService.GetItemAsync<JwtSecurityToken>("AccessToken").Returns((JwtSecurityToken)null);

        (await provider.GetName()).Should().BeNull();
    }
}
