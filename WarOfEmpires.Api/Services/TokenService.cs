using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using WarOfEmpires.Api.Configuration;
using WarOfEmpires.Models.Security;
using WarOfEmpires.Api.Routing;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using WarOfEmpires.Domain.Security;

namespace WarOfEmpires.Api.Services {
    public sealed class TokenService : ITokenService {
        private readonly ClientSettings clientSettings;
        private readonly SecurityKey issuerSigningKey;
        private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler;

        public TokenService(ClientSettings clientSettings, SecurityKey issuerSigningKey, JwtSecurityTokenHandler jwtSecurityTokenHandler) {
            this.clientSettings = clientSettings;
            this.issuerSigningKey = issuerSigningKey;
            this.jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        }

        public string CreateToken(UserClaimsViewModel viewModel) {
            var now = DateTime.UtcNow;
            var descriptor = new SecurityTokenDescriptor() {
                Audience = clientSettings.TokenAudience,
                Issuer = clientSettings.TokenIssuer,
                Subject = new ClaimsIdentity(GetClaims(viewModel)),
                IssuedAt = now,
                NotBefore = now,
                Expires = now.AddMinutes(clientSettings.TokenExpirationTimeInMinutes),
                SigningCredentials = new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var accessToken = jwtSecurityTokenHandler.CreateToken(descriptor);

            return jwtSecurityTokenHandler.WriteToken(accessToken);
        }

        private IEnumerable<Claim> GetClaims(UserClaimsViewModel viewModel) {
            yield return new Claim(JwtRegisteredClaimNames.Sub, viewModel.Subject);

            if (viewModel.DisplayName != null) {
                yield return new Claim(JwtRegisteredClaimNames.Name, viewModel.DisplayName);
            }

            if (viewModel.IsAdmin) {
                yield return new Claim(Roles.ClaimName, Roles.Administrator);
            }

            if (viewModel.IsPlayer) {
                yield return new Claim(Roles.ClaimName, Roles.Player);
            }

            // TODO add more roles/claims
        }

        public bool TryGetIdentity(string token, [NotNullWhen(true)] out string? identity) {
            var validationParameters = new TokenValidationParameters() {
                RequireAudience = true,
                ValidateAudience = true,
                ValidAudience = clientSettings.TokenAudience,
                ValidateIssuer = true,
                ValidIssuer = clientSettings.TokenIssuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = issuerSigningKey,
                RequireExpirationTime = true,
                ValidateLifetime = false // Expired tokens are explicitly allowed
            };

            if (jwtSecurityTokenHandler.CanReadToken(token)) {
                try {
                    var principal = jwtSecurityTokenHandler.ValidateToken(token, validationParameters, out _);

                    identity = principal.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value;
                    return true;
                }
                catch { }
            }

            identity = null;
            return false;
        }
    }
}
