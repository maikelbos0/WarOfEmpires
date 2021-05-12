using System.Threading.Tasks;

namespace WarOfEmpires.Services {
    public interface IAuthenticationService {
        bool IsAuthenticated { get; }
        string Identity { get; }
        Task SignIn(string identity);
        Task SignOut();
    }
}