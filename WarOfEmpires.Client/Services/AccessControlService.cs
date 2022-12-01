using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WarOfEmpires.Client.Services;

public sealed class AccessControlService : AuthenticationStateProvider, IAccessControlService {
    private readonly ILocalStorageService storageService;
    private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler;
    private readonly ITimerService timerService;
    private bool isTimerStarted = false;

    public AccessControlService(ILocalStorageService storageService, ITimerService timerService, JwtSecurityTokenHandler jwtSecurityTokenHandler) {
        this.storageService = storageService;
        this.timerService = timerService;
        this.jwtSecurityTokenHandler = jwtSecurityTokenHandler;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
        var accessToken = await GetValidToken();

        if (accessToken == null) {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
        else {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(accessToken.Claims, Constants.AuthenticationScheme, JwtRegisteredClaimNames.Name, Api.Routing.Roles.ClaimName)));
        }
    }

    public async Task SignIn(string accessToken, string refreshToken) {
        await storageService.SetItemAsync(Constants.AccessToken, accessToken);
        await storageService.SetItemAsync(Constants.RefreshToken, refreshToken);

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task SignOut() {
        await storageService.RemoveItemAsync(Constants.AccessToken);
        await storageService.RemoveItemAsync(Constants.RefreshToken);

        isTimerStarted = false;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private async Task<JwtSecurityToken?> GetValidToken() {
        var token = await storageService.GetItemAsync<string?>(Constants.AccessToken);

        if (token == null) {
            return null;
        }

        var accessToken = jwtSecurityTokenHandler.ReadJwtToken(token);

        if (accessToken.ValidTo < DateTime.UtcNow) {
            await storageService.RemoveItemAsync(Constants.AccessToken);

            return null;
        }

        if (!isTimerStarted) {
            isTimerStarted = true;
            timerService.ExecuteAfter(SignOut, accessToken.ValidTo - DateTime.UtcNow);
        }

        return accessToken;
    }
}
