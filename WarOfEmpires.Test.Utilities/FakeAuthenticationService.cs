using System.Threading.Tasks;
using WarOfEmpires.Services;

namespace WarOfEmpires.Test.Utilities {
    public sealed class FakeAuthenticationService : IAuthenticationService {
        public bool IsAuthenticated {
            get {
                return Identity != null;
            }
        }
        public string Identity { get; set; }

        public string GetUserName() {
            return Identity;
        }

        public Task SignIn(string identity) {
            Identity = identity;

            return Task.CompletedTask;
        }

        public Task SignOut() {
            Identity = null;

            return Task.CompletedTask;
        }
    }
}
