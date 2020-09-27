using WarOfEmpires.Attributes;

namespace WarOfEmpires.Services {
    public interface IAuthorizationService {
        bool IsAdmin();
        bool IsAuthorized(IAllianceAuthorizeAttribute attribute);
    }
}