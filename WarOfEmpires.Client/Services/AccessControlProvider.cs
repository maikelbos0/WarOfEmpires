using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using WarOfEmpires.Api.Routing;

namespace WarOfEmpires.Client.Services;

public sealed class AccessControlProvider {
    private const string StorageKey = "AccessToken";

    private readonly ILocalStorageService storageService;

    public delegate void AccessControlStateChangedHandler(AccessControlState state);

    public event AccessControlStateChangedHandler? AccessControlStateChanged;

    public AccessControlProvider(ILocalStorageService storageService) {
        this.storageService = storageService;
    }

    public async Task SignIn(string token) {
        var tokenHandler = new JwtSecurityTokenHandler();
        var accessToken = tokenHandler.ReadJwtToken(token);
        
        await storageService.SetItemAsync(StorageKey, accessToken);

        AccessControlStateChanged?.Invoke(new AccessControlState(
            accessToken.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value,
            true,
            accessToken.Claims.Any(c => c.Type == Roles.ClaimName && c.Value == Roles.Administrator)
        ));
    }

    public async Task SignOut() {
        await storageService.RemoveItemAsync(StorageKey);

        AccessControlStateChanged?.Invoke(new AccessControlState());
    }

    public async Task<bool> IsAuthenticated() => await GetAccessToken() != null;

    public async Task<string?> GetName() => (await GetAccessToken())?.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value;

    private async Task<JwtSecurityToken?> GetAccessToken() => await storageService.GetItemAsync<JwtSecurityToken?>(StorageKey);
}
