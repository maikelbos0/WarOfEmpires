using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using WarOfEmpires.Api.Configuration;

namespace WarOfEmpires.Api.Services {
    public sealed class TokenService : ITokenService {
        private readonly ClientSettings clientSettings;
        private readonly SecurityKey issuerSigningKey;

        public TokenService(ClientSettings clientSettings, SecurityKey issuerSigningKey) {
            this.clientSettings = clientSettings;
            this.issuerSigningKey = issuerSigningKey;
        }

        public string CreateToken(string subject, bool isAdmin) {
            var now = DateTime.UtcNow;
            var handler = new JwtSecurityTokenHandler();
            var descriptor = new SecurityTokenDescriptor() {
                IssuedAt = now,
                NotBefore = now,
                Expires = now.AddMinutes(clientSettings.TokenExpirationTimeInMinutes),
                Audience = clientSettings.TokenAudience,
                Issuer = clientSettings.TokenIssuer,
                SigningCredentials = new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256Signature),
                Claims = new Dictionary<string, object>() {
                    { JwtRegisteredClaimNames.Sub, subject }
                }
            };

            if (isAdmin) {
                descriptor.Claims.Add(ClaimTypes.Role, Roles.Administrator);
            }

            var token = handler.CreateToken(descriptor);

            return handler.WriteToken(token);
        }
    }
}
