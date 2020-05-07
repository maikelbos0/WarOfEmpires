namespace WarOfEmpires.Services {
    public interface IAuthenticationService {
        bool IsAuthenticated { get; }
        string Identity { get; }
        bool IsAdmin();
        void SignIn(string identity);
        void SignOut();
    }
}