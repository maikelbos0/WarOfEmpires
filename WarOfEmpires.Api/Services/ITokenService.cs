using WarOfEmpires.Models.Security;

namespace WarOfEmpires.Api.Services {
    public interface ITokenService {
        string CreateToken(UserClaimsViewModel viewModel);
        bool TryGetIdentity(string token, out string? identity);
    }
}