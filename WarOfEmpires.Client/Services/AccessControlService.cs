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
    private readonly ITokenService tokenService;

    public AccessControlService(ILocalStorageService storageService, JwtSecurityTokenHandler jwtSecurityTokenHandler, ITokenService tokenService) {
        this.storageService = storageService;
        this.jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        this.tokenService = tokenService;
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
        await storageService.SetItemAsync(Constants.Tokens, tokens);

        NotifyAuthenticationStateChanged(Task.FromResult(GetAuthenticatedState(tokens.AccessToken)));
    }

    public async Task SignOut() {
        await storageService.RemoveItemAsync(Constants.Tokens);

        NotifyAuthenticationStateChanged(Task.FromResult(GetUnauthenticatedState()));
    }

    private static AuthenticationState GetUnauthenticatedState() 
        => new(new ClaimsPrincipal(new ClaimsIdentity()));

    private AuthenticationState GetAuthenticatedState(string accessToken)
        => new(new ClaimsPrincipal(new ClaimsIdentity(jwtSecurityTokenHandler.ReadJwtToken(accessToken).Claims, Constants.AuthenticationScheme, JwtRegisteredClaimNames.Name, Roles.ClaimName)));

    public async Task<string?> GetAccessToken() {
        var tokens = await storageService.GetItemAsync<UserTokenModel?>(Constants.Tokens);

        if (tokens == null) {
            return null;
        }

        var accessToken = jwtSecurityTokenHandler.ReadJwtToken(tokens.AccessToken);

        if (accessToken.ValidTo < DateTime.UtcNow.AddSeconds(5)) {
            return await AcquireNewTokens(tokens);
        }

        return tokens.AccessToken;
    }

    private async Task<string?> AcquireNewTokens(UserTokenModel tokens) {
        var newTokens = await tokenService.AcquireNewTokens(tokens);

        if (newTokens != null) {
            await storageService.SetItemAsync(Constants.Tokens, newTokens);
            return newTokens.AccessToken;
        }

        await storageService.RemoveItemAsync(Constants.Tokens);
        NotifyAuthenticationStateChanged(Task.FromResult(GetUnauthenticatedState()));
        return null;
    }
}
