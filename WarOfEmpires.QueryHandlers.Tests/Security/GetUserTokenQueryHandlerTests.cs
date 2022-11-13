using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.QueryHandlers.Security;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Authorization;
using WarOfEmpires.Utilities.Configuration;

namespace WarOfEmpires.QueryHandlers.Tests.Security;

[TestClass]
public class GetUserTokenQueryHandlerTests {
    [TestMethod]
    public void GetUserTokenQueryHandler_Returns_Valid_Token() {
        var clientSettings = new ClientSettings() {
            TokenAudience = "Audience",
            TokenIssuer = "Issuer",
            TokenExpirationTimeInMinutes = 60
        };
        var signingKey = new SymmetricSecurityKey(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7 });
        var builder = new FakeBuilder()
            .BuildPlayer(1);

        builder.User.IsAdmin.Returns(true);

        var handler = new GetUserTokenQueryHandler(builder.Context, clientSettings, signingKey);
        var query = new GetUserTokenQuery("test1@test.com");

        var result = handler.Execute(query);

        var tokenHandler = new JwtSecurityTokenHandler();
        tokenHandler.ValidateToken(result, new TokenValidationParameters() {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey
        }, out var token);
        
        int secondsSinceEpoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;

        var jwtToken = token.Should().BeAssignableTo<JwtSecurityToken>().Subject;
        jwtToken.Issuer.Should().Be("Issuer");
        jwtToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Aud).Which.Value.Should().Be("Audience");
        
        AssertTimestamp(jwtToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Iat).Which.Value, DateTime.UtcNow);
        AssertTimestamp(jwtToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Nbf).Which.Value, DateTime.UtcNow);
        AssertTimestamp(jwtToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Exp).Which.Value, DateTime.UtcNow.AddMinutes(clientSettings.TokenExpirationTimeInMinutes));

        void AssertTimestamp(string timestampValue, DateTime expectedDate) {
            int.TryParse(timestampValue, out var timestamp).Should().BeTrue();
            var expectedTimestamp = (int)(expectedDate - new DateTime(1970, 1, 1)).TotalSeconds;

            timestamp.Should().BeCloseTo(expectedTimestamp, 1);
        }
    }

    [TestMethod]
    public void GetUserTokenQueryHandler_Returns_Token_With_Administrator_Role_For_Administrators() {
        var signingKey = new SymmetricSecurityKey(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7 });
        var builder = new FakeBuilder()
            .BuildPlayer(1);

        builder.User.IsAdmin.Returns(true);

        var handler = new GetUserTokenQueryHandler(builder.Context, new ClientSettings() { TokenExpirationTimeInMinutes = 60 }, signingKey);
        var query = new GetUserTokenQuery("test1@test.com");

        var result = handler.Execute(query);
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadToken(result).Should().BeAssignableTo<JwtSecurityToken>().Subject;

        token.Claims.Should().Contain(c => c.Type == "role").Which.Value.Should().Be(Roles.Administrator);
    }

    [TestMethod]
    public void GetUserTokenQueryHandler_Returns_Token_Without_Administrator_Role_For_Normal_User() {
        var signingKey = new SymmetricSecurityKey(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7 });
        var builder = new FakeBuilder()
            .BuildPlayer(1);

        builder.User.IsAdmin.Returns(false);

        var handler = new GetUserTokenQueryHandler(builder.Context, new ClientSettings() { TokenExpirationTimeInMinutes = 60 }, signingKey);
        var query = new GetUserTokenQuery("test1@test.com");

        var result = handler.Execute(query);
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadToken(result).Should().BeAssignableTo<JwtSecurityToken>().Subject;
        
        token.Claims.Should().NotContain(c => c.Type == "role");
    }
}
