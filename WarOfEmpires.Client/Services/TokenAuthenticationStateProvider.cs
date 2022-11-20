using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WarOfEmpires.Client.Services;

public sealed class TokenAuthenticationStateProvider : AuthenticationStateProvider {
    private readonly IAccessControlProvider accessControlProvider;

    public TokenAuthenticationStateProvider(IAccessControlProvider accessControlProvider) {
        this.accessControlProvider = accessControlProvider;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
        var accessToken = await accessControlProvider.GetValidToken();

        if (accessToken == null) {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
        else {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(accessToken.Claims, Constants.AuthenticationScheme)));
        }
    }
}
