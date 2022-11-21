using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WarOfEmpires.Client.Services;

public sealed class TokenAuthenticationStateProvider : AuthenticationStateProvider, IDisposable {
    private readonly IAccessControlProvider accessControlProvider;

    public TokenAuthenticationStateProvider(IAccessControlProvider accessControlProvider) {
        this.accessControlProvider = accessControlProvider;
        
        accessControlProvider.AccessControlStateChanged += HandleAccessControlStateChanged;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
        var accessToken = await accessControlProvider.GetValidToken();
        
        if (accessToken == null) {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
        else {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(accessToken.Claims, Constants.AuthenticationScheme, JwtRegisteredClaimNames.Name, Api.Routing.Roles.ClaimName)));
        }
    }

    private Task HandleAccessControlStateChanged(AccessControlState state) {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

        return Task.CompletedTask;
    }

    public void Dispose() {
        accessControlProvider.AccessControlStateChanged -= HandleAccessControlStateChanged;
    }
}
