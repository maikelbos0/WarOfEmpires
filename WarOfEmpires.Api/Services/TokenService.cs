using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using WarOfEmpires.Api.Configuration;
using WarOfEmpires.Models.Security;
using WarOfEmpires.Api.Routing;

namespace WarOfEmpires.Api.Services {
    public sealed class TokenService : ITokenService {
        private readonly ClientSettings clientSettings;
        private readonly SecurityKey issuerSigningKey;

        public TokenService(ClientSettings clientSettings, SecurityKey issuerSigningKey) {
            this.clientSettings = clientSettings;
            this.issuerSigningKey = issuerSigningKey;
        }

        public string CreateToken(UserClaimsViewModel viewModel) {
            var now = DateTime.UtcNow;
            var handler = new JwtSecurityTokenHandler();
            var claims = new List<Claim>() {
                new Claim(JwtRegisteredClaimNames.Sub, viewModel.Subject),
                new Claim(JwtRegisteredClaimNames.Name, viewModel.DisplayName),
            };

            if (viewModel.IsAdmin) {
                claims.Add(new Claim(ClaimTypes.Role, Roles.Administrator));
            }

            // TODO add more roles/claims

            var descriptor = new SecurityTokenDescriptor() {
                Audience = clientSettings.TokenAudience,
                Issuer = clientSettings.TokenIssuer,
                Subject = new ClaimsIdentity(claims),
                IssuedAt = now,
                NotBefore = now,
                Expires = now.AddMinutes(clientSettings.TokenExpirationTimeInMinutes),
                SigningCredentials = new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = handler.CreateToken(descriptor);

            return handler.WriteToken(token);
        }
    }
}
