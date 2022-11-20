using Blazored.LocalStorage;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using WarOfEmpires.Api.Routing;

namespace WarOfEmpires.Client.Services;

public sealed class AccessControlProvider {
    private readonly ILocalStorageService storageService;
    private readonly ITimerService timerService;
    private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler;
    private bool isTimerStarted = false;

    public delegate void AccessControlStateChangedHandler(AccessControlState state);

    public event AccessControlStateChangedHandler? AccessControlStateChanged;

    public AccessControlProvider(ILocalStorageService storageService, ITimerService timerService, JwtSecurityTokenHandler jwtSecurityTokenHandler) {
        this.storageService = storageService;
        this.timerService = timerService;
        this.jwtSecurityTokenHandler = jwtSecurityTokenHandler;
    }

    public async Task SignIn(string token) {
        await storageService.SetItemAsync(Constants.AccessToken, token);

        AccessControlStateChanged?.Invoke(await GetAccessControlState(token));
    }

    public async Task SignOut() {
        await storageService.RemoveItemAsync(Constants.AccessToken);

        AccessControlStateChanged?.Invoke(await GetAccessControlState(null));
        isTimerStarted = false;
    }

    public async Task<AccessControlState> GetAccessControlState() {
        var token = await storageService.GetItemAsync<string?>(Constants.AccessToken);

        return await GetAccessControlState(token);
    }

    private async Task<AccessControlState> GetAccessControlState(string? token) {
        if (token == null) {
            return new AccessControlState();
        }

        var accessToken = await GetValidToken(token);

        if (accessToken == null) {
            return new AccessControlState();
        }

        var displayName = accessToken.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value;
        var isAdmin = accessToken.Claims.Any(c => c.Type == Roles.ClaimName && c.Value == Roles.Administrator);

        return new AccessControlState(displayName, true, isAdmin);
    }

    private async Task<JwtSecurityToken?> GetValidToken(string token) {
        var accessToken = jwtSecurityTokenHandler.ReadJwtToken(token);

        if (accessToken.ValidTo < DateTime.UtcNow) {
            await storageService.RemoveItemAsync(Constants.AccessToken);

            return null;
        }

        if (!isTimerStarted) {
            timerService.ExecuteAfter(SignOut, accessToken.ValidTo - DateTime.UtcNow);
            isTimerStarted = true;
        }

        return accessToken;
    }
}
