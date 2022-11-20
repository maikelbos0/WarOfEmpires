using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace WarOfEmpires.Client.Services;

public interface IAccessControlProvider {
    event AccessControlStateChangedHandler? AccessControlStateChanged;

    Task<AccessControlState> GetAccessControlState();
    Task<JwtSecurityToken?> GetValidToken();
    Task SignIn(string token);
    Task SignOut();
}