using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IdentityModel.Tokens.Jwt;
using WarOfEmpires.Api.Configuration;
using WarOfEmpires.Api.Routing;
using WarOfEmpires.Api.Services;
using WarOfEmpires.Models.Security;

namespace WarOfEmpires.Api.Tests.Services {
    [TestClass]
    public class TokenServiceTests {
        [TestMethod]
        public void CreateToken_Returns_Valid_Token() {
            var clientSettings = new ClientSettings() {
                TokenAudience = "Audience",
                TokenIssuer = "Issuer",
                TokenExpirationTimeInMinutes = 60
            };
            var signingKey = new SymmetricSecurityKey(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7 });
            var service = new TokenService(clientSettings, signingKey, new JwtSecurityTokenHandler());

            var result = service.CreateToken(new UserClaimsViewModel() {
                Subject = "test@test.com"
            });

            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(result, new TokenValidationParameters() {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey
            }, out var token);

            var jwtToken = token.Should().BeAssignableTo<JwtSecurityToken>().Subject;
            jwtToken.Issuer.Should().Be("Issuer");
            jwtToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Aud).Which.Value.Should().Be("Audience");
            jwtToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Sub).Which.Value.Should().Be("test@test.com");
            jwtToken.Claims.Should().NotContain(c => c.Type == JwtRegisteredClaimNames.Name);

            AssertTimestamp(jwtToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Iat).Which.Value, DateTime.UtcNow);
            AssertTimestamp(jwtToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Nbf).Which.Value, DateTime.UtcNow);
            AssertTimestamp(jwtToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Exp).Which.Value, DateTime.UtcNow.AddMinutes(clientSettings.TokenExpirationTimeInMinutes));

            static void AssertTimestamp(string timestampValue, DateTime expectedDate) {
                int.TryParse(timestampValue, out var timestamp).Should().BeTrue();
                var expectedTimestamp = (int)(expectedDate - new DateTime(1970, 1, 1)).TotalSeconds;

                timestamp.Should().BeCloseTo(expectedTimestamp, 1);
            }
        }

        [TestMethod]
        public void CreateToken_Returns_Token_With_Name_For_DisplayName() {
            var signingKey = new SymmetricSecurityKey(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7 });
            var service = new TokenService(new ClientSettings() { TokenExpirationTimeInMinutes = 60 }, signingKey, new JwtSecurityTokenHandler());

            var result = service.CreateToken(new UserClaimsViewModel() {
                Subject = "test@test.com",
                DisplayName = "Test"
            });
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadToken(result).Should().BeAssignableTo<JwtSecurityToken>().Subject;

            token.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Name).Which.Value.Should().Be("Test");
        }

        [TestMethod]
        public void CreateToken_Returns_Token_With_Administrator_Role_For_Administrators() {
            var signingKey = new SymmetricSecurityKey(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7 });
            var service = new TokenService(new ClientSettings() { TokenExpirationTimeInMinutes = 60 }, signingKey, new JwtSecurityTokenHandler());

            var result = service.CreateToken(new UserClaimsViewModel() {
                Subject = "test@test.com",
                IsAdmin = true
            });
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadToken(result).Should().BeAssignableTo<JwtSecurityToken>().Subject;

            token.Claims.Should().Contain(c => c.Type == Roles.ClaimName).Which.Value.Should().Be(Roles.Administrator);
        }

        [TestMethod]
        public void CreateToken_Returns_Token_Without_Administrator_Role_For_Normal_User() {
            var signingKey = new SymmetricSecurityKey(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7 });
            var service = new TokenService(new ClientSettings() { TokenExpirationTimeInMinutes = 60 }, signingKey, new JwtSecurityTokenHandler());

            var result = service.CreateToken(new UserClaimsViewModel() {
                Subject = "test@test.com",
                IsAdmin = false
            });
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadToken(result).Should().BeAssignableTo<JwtSecurityToken>().Subject;

            token.Claims.Should().NotContain(c => c.Type == Roles.ClaimName);
        }

        [TestMethod]
        public void CreateToken_Returns_Token_With_Player_Role_For_Player() {
            var signingKey = new SymmetricSecurityKey(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7 });
            var service = new TokenService(new ClientSettings() { TokenExpirationTimeInMinutes = 60 }, signingKey, new JwtSecurityTokenHandler());

            var result = service.CreateToken(new UserClaimsViewModel() {
                Subject = "test@test.com",
                IsPlayer = true
            });
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadToken(result).Should().BeAssignableTo<JwtSecurityToken>().Subject;

            token.Claims.Should().Contain(c => c.Type == Roles.ClaimName).Which.Value.Should().Be(Roles.Player);
        }

        [TestMethod]
        public void CreateToken_Returns_Token_Without_Player_Role_For_New_User() {
            var signingKey = new SymmetricSecurityKey(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7 });
            var service = new TokenService(new ClientSettings() { TokenExpirationTimeInMinutes = 60 }, signingKey, new JwtSecurityTokenHandler());

            var result = service.CreateToken(new UserClaimsViewModel() {
                Subject = "test@test.com",
                IsPlayer = false
            });
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadToken(result).Should().BeAssignableTo<JwtSecurityToken>().Subject;

            token.Claims.Should().NotContain(c => c.Type == Roles.ClaimName);
        }

        [TestMethod]
        public void TryGetIdentity_Returns_True_When_Identity_Found_In_Expired_Token() {
            var clientSettings = new ClientSettings() {
                TokenAudience = "Audience",
                TokenIssuer = "Issuer"
            };
            var signingKey = new SymmetricSecurityKey(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7 });
            var service = new TokenService(clientSettings, signingKey, new JwtSecurityTokenHandler());

            service.TryGetIdentity("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ0ZXN0QHRlc3QuY29tIiwibmJmIjoxNjY5ODM3MzgwLCJleHAiOjE2Njk4Mzc0NDAsImlhdCI6MTY2OTgzNzM4MCwiaXNzIjoiSXNzdWVyIiwiYXVkIjoiQXVkaWVuY2UifQ.zddYMO1vjVQdgvKJu2c7b0vnW0oIAS3K9RNc06xtzII", out var identity).Should().BeTrue();
            identity.Should().Be("test@test.com");
        }

        [TestMethod]
        public void TryGetIdentity_Returns_False_When_Identity_Not_Found() {
            var clientSettings = new ClientSettings() {
                TokenAudience = "Audience",
                TokenIssuer = "Issuer"
            };
            var signingKey = new SymmetricSecurityKey(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7 });
            var service = new TokenService(clientSettings, signingKey, new JwtSecurityTokenHandler());

            service.TryGetIdentity("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2Njk4MzgwNjYsImV4cCI6MTY2OTgzODEyNiwiaWF0IjoxNjY5ODM4MDY2LCJpc3MiOiJJc3N1ZXIiLCJhdWQiOiJBdWRpZW5jZSJ9.3ETUZuIT5v69qpWyX2z-VzZnPPDR5UufwXxXG1VkY7E", out var identity).Should().BeFalse();
            identity.Should().BeNull();
        }

        [TestMethod]
        public void TryGetIdentity_Returns_False_When_Signature_Invalid() {
            var clientSettings = new ClientSettings() {
                TokenAudience = "Audience",
                TokenIssuer = "Issuer"
            };
            var signingKey = new SymmetricSecurityKey(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 0 });
            var service = new TokenService(clientSettings, signingKey, new JwtSecurityTokenHandler());

            service.TryGetIdentity("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ0ZXN0QHRlc3QuY29tIiwibmJmIjoxNjY5ODM3MzgwLCJleHAiOjE2Njk4Mzc0NDAsImlhdCI6MTY2OTgzNzM4MCwiaXNzIjoiSXNzdWVyIiwiYXVkIjoiQXVkaWVuY2UifQ.zddYMO1vjVQdgvKJu2c7b0vnW0oIAS3K9RNc06xtzII", out var identity).Should().BeFalse();
            identity.Should().BeNull();
        }

        [TestMethod]
        public void TryGetIdentity_Returns_False_When_Token_Is_Invalid() {
            var clientSettings = new ClientSettings() {
                TokenAudience = "Audience",
                TokenIssuer = "Issuer"
            };
            var signingKey = new SymmetricSecurityKey(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7 });
            var service = new TokenService(clientSettings, signingKey, new JwtSecurityTokenHandler());

            service.TryGetIdentity("not a token", out var identity).Should().BeFalse();
            identity.Should().BeNull();
        }
    }
}
