using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;
using WarOfEmpires.Models.Security;

namespace WarOfEmpires.Client.Services {
    public interface IAccessControlService {
        event AuthenticationStateChangedHandler? AuthenticationStateChanged;
        Task<AuthenticationState> GetAuthenticationStateAsync();
        Task SignIn(UserTokenModel tokens);
        Task SignOut();
    }
}