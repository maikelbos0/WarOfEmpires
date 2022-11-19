using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using WarOfEmpires.Api.Routing;

namespace WarOfEmpires.Client.Services;

public sealed class AccessControlProvider {
    public const string StorageKey = "AccessToken";

    private readonly ILocalStorageService storageService;

    public delegate void AccessControlStateChangedHandler(AccessControlState state);

    public event AccessControlStateChangedHandler? AccessControlStateChanged;

    public AccessControlProvider(ILocalStorageService storageService) {
        this.storageService = storageService;
    }

    public async Task SignIn(string token) {
        await storageService.SetItemAsync(StorageKey, token);

        AccessControlStateChanged?.Invoke(GetAccessControlState(token));
    }

    public async Task SignOut() {
        await storageService.RemoveItemAsync(StorageKey);

        AccessControlStateChanged?.Invoke(GetAccessControlState(null));
    }

    public async Task<AccessControlState> GetAccessControlState() {
        var token = await storageService.GetItemAsync<string?>(StorageKey);

        return GetAccessControlState(token);
    }

    private AccessControlState GetAccessControlState(string? token) {
        if (token == null) {
            return new AccessControlState();
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var accessToken = tokenHandler.ReadJwtToken(token);
        var displayName = accessToken.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value;
        var isAdmin = accessToken.Claims.Any(c => c.Type == Roles.ClaimName && c.Value == Roles.Administrator);

        return new AccessControlState(displayName, true, isAdmin);
    }
}
