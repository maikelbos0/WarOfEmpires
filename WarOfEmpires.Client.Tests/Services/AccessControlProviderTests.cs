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

        await provider.SignIn("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ0ZXN0QHRlc3QuY29tIiwibmFtZSI6IlRlc3QiLCJuYmYiOjE2Njg0NTU4MjksImV4cCI6MTY2ODQ1OTQyOSwiaWF0IjoxNjY4NDU1ODI5LCJpc3MiOiJJc3N1ZXIiLCJhdWQiOiJBdWRpZW5jZSJ9.YkMn7n5esCSRHclW6ffXMQp5_aBki5UJUm9u2ry111I");

        await storageService.Received().SetItemAsync("AccessToken", Arg.Is<JwtSecurityToken>(t => t.Issuer == "Issuer"));
    }

    [TestMethod]
    public async Task SignOut() {
        var storageService = Substitute.For<ILocalStorageService>();
        var provider = new AccessControlProvider(storageService);

        await provider.SignOut();

        await storageService.Received().RemoveItemAsync("AccessToken");
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
