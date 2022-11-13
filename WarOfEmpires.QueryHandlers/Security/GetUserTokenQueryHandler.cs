using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using WarOfEmpires.Database;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.Utilities.Authorization;
using WarOfEmpires.Utilities.Configuration;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Security {
    public sealed class GetUserTokenQueryHandler : IQueryHandler<GetUserTokenQuery, string> {
        private readonly IReadOnlyWarContext _context;
        private readonly ClientSettings _clientSettings;
        private readonly SecurityKey _issuerSigningKey;

        public GetUserTokenQueryHandler(IReadOnlyWarContext context, ClientSettings clientSettings, SecurityKey issuerSigningKey) {
            _context = context;
            _clientSettings = clientSettings;
            _issuerSigningKey = issuerSigningKey;
        }

        public string Execute(GetUserTokenQuery query) {
            var now = DateTime.UtcNow;
            var isAdmin = _context.Users
                .Where(p => EmailComparisonService.Equals(p.Email, query.Email))
                .Select(p => p.IsAdmin)
                .Single();
            var handler = new JwtSecurityTokenHandler();
            var descriptor = new SecurityTokenDescriptor() {
                IssuedAt = now,
                NotBefore = now,
                Expires = now.AddMinutes(_clientSettings.TokenExpirationTimeInMinutes),
                Audience = _clientSettings.TokenAudience,
                Issuer = _clientSettings.TokenIssuer,
                SigningCredentials = new SigningCredentials(_issuerSigningKey, SecurityAlgorithms.HmacSha256Signature)
            };

            if (isAdmin) {
                descriptor.Claims = new Dictionary<string, object>() { { ClaimTypes.Role, Roles.Administrator } };
            }

            var token = handler.CreateToken(descriptor);
            
            return handler.WriteToken(token);
        }
    }
}