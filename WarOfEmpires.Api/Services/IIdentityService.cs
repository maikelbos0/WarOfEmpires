namespace WarOfEmpires.Api.Services;

public interface IIdentityService {
    bool IsAuthenticated { get; }
    string Identity { get; }
}
