namespace WarOfEmpires.Services {
    public interface IAuthenticationService {
        bool IsAuthenticated { get; }
        string Identity { get; }
        string GetUserName();
        bool IsAdmin();
        bool IsInAlliance();
        void SignIn(string identity);
        void SignOut();
    }
}