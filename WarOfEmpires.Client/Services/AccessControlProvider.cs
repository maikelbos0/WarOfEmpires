using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using WarOfEmpires.Api.Routing;

namespace WarOfEmpires.Client.Services;

public sealed class AccessControlProvider {
    private readonly ILocalStorageService storageService;
    private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler;

    public delegate void AccessControlStateChangedHandler(AccessControlState state);

    public event AccessControlStateChangedHandler? AccessControlStateChanged;

    public AccessControlProvider(ILocalStorageService storageService, JwtSecurityTokenHandler jwtSecurityTokenHandler) {
        this.storageService = storageService;
        this.jwtSecurityTokenHandler = jwtSecurityTokenHandler;
    }

    public async Task SignIn(string token) {
        await storageService.SetItemAsync(Constants.AccessToken, token);

        AccessControlStateChanged?.Invoke(GetAccessControlState(token));
    }

    public async Task SignOut() {
        await storageService.RemoveItemAsync(Constants.AccessToken);

        AccessControlStateChanged?.Invoke(GetAccessControlState(null));
    }

    public async Task<AccessControlState> GetAccessControlState() {
        var token = await storageService.GetItemAsync<string?>(Constants.AccessToken);

        return GetAccessControlState(token);
    }

    private AccessControlState GetAccessControlState(string? token) {
        if (token == null) {
            return new AccessControlState();
        }

        var accessToken = jwtSecurityTokenHandler.ReadJwtToken(token);
        var displayName = accessToken.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value;
        var isAdmin = accessToken.Claims.Any(c => c.Type == Roles.ClaimName && c.Value == Roles.Administrator);

        return new AccessControlState(displayName, true, isAdmin);
    }
}
