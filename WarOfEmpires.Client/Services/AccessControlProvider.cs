using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace WarOfEmpires.Client.Services;

public class AccessControlProvider {
    private const string StorageKey = "AccessToken";

    private readonly ILocalStorageService storageService;

    public AccessControlProvider(ILocalStorageService storageService) {
        this.storageService = storageService;
    }

    public async Task SignIn(string token) {
        var tokenHandler = new JwtSecurityTokenHandler();
        var accessToken = tokenHandler.ReadJwtToken(token);

        await storageService.SetItemAsync(StorageKey, accessToken);
    }

    public async Task SignOut() {
        await storageService.RemoveItemAsync(StorageKey);
    }

    public async Task<bool> IsAuthenticated() => await GetAccessToken() != null;

    private async Task<JwtSecurityToken?> GetAccessToken() => await storageService.GetItemAsync<JwtSecurityToken?>(StorageKey);
}
