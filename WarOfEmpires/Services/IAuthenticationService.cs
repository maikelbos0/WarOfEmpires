namespace WarOfEmpires.Services {
    public interface IAuthenticationService {
        bool IsAuthenticated { get; }
        string Identity { get; }
        void SignIn(string identity);
        void SignOut();
    }
}