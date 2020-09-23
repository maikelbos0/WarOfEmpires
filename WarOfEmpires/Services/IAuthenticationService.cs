using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Services {
    public interface IAuthenticationService {
        bool IsAuthenticated { get; }
        string Identity { get; }
        bool IsAdmin();
        CurrentAllianceRightsViewModel GetAllianceRights();
        void SignIn(string identity);
        void SignOut();
    }
}