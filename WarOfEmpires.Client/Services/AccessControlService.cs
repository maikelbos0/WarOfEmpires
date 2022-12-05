using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using WarOfEmpires.Api.Routing;
using WarOfEmpires.Models.Security;

namespace WarOfEmpires.Client.Services;

public sealed class AccessControlService : AuthenticationStateProvider, IAccessControlService {
    private readonly ILocalStorageService storageService;
    private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler;
    private readonly IHttpService httpService;

    public AccessControlService(ILocalStorageService storageService, JwtSecurityTokenHandler jwtSecurityTokenHandler, IHttpService httpService) {
        this.storageService = storageService;
        this.jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        this.httpService = httpService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
        var accessToken = await GetAccessToken();

        if (accessToken == null) {
            return GetUnauthenticatedState();
        }
        else {
            return GetAuthenticatedState(accessToken);
        }
    }

    public async Task SignIn(UserTokenModel tokens) {
        var accessToken = jwtSecurityTokenHandler.ReadJwtToken(tokens.AccessToken);

        await storageService.SetItemAsync(Constants.Tokens, tokens);

        NotifyAuthenticationStateChanged(Task.FromResult(GetAuthenticatedState(accessToken)));
    }

    public async Task SignOut() {
        await storageService.RemoveItemAsync(Constants.Tokens);

        NotifyAuthenticationStateChanged(Task.FromResult(GetUnauthenticatedState()));
    }

    private static AuthenticationState GetUnauthenticatedState() 
        => new(new ClaimsPrincipal(new ClaimsIdentity()));

    private static AuthenticationState GetAuthenticatedState(JwtSecurityToken accessToken)
        => new(new ClaimsPrincipal(new ClaimsIdentity(accessToken.Claims, Constants.AuthenticationScheme, JwtRegisteredClaimNames.Name, Roles.ClaimName)));

    private async Task<JwtSecurityToken?> GetAccessToken() {
        var tokens = await storageService.GetItemAsync<UserTokenModel?>(Constants.Tokens);

        if (tokens == null) {
            return null;
        }

        var accessToken = jwtSecurityTokenHandler.ReadJwtToken(tokens.AccessToken);

        if (accessToken.ValidTo < DateTime.UtcNow) {
            return await AcquireNewAccessToken(tokens);
        }

        return accessToken;
    }

    private async Task<JwtSecurityToken?> AcquireNewAccessToken(UserTokenModel tokens) {
        var newTokens = await httpService.PostAsync<UserTokenModel, UserTokenModel>(Security.AcquireToken, tokens);

        if (newTokens != null) {
            await storageService.SetItemAsync(Constants.Tokens, newTokens);
            return jwtSecurityTokenHandler.ReadJwtToken(newTokens.AccessToken);
        }

        await storageService.RemoveItemAsync(Constants.Tokens);
        NotifyAuthenticationStateChanged(Task.FromResult(GetUnauthenticatedState()));
        return null;
    }
}
