namespace WarOfEmpires.Services {
    public interface IAuthorizationService {
        bool IsAdmin();
        bool IsAuthorized(IAllianceAuthorizationRequest request);
    }
}