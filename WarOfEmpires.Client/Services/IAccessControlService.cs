using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;

namespace WarOfEmpires.Client.Services {
    public interface IAccessControlService {
        event AuthenticationStateChangedHandler? AuthenticationStateChanged;
        Task<AuthenticationState> GetAuthenticationStateAsync();
        Task SignIn(string token);
        Task SignOut();
    }
}