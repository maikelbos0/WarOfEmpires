using WarOfEmpires.Models.Security;

namespace WarOfEmpires.Api.Services {
    public interface ITokenService {
        string CreateToken(UserClaimsViewModel viewModel);
    }
}