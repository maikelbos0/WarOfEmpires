using Microsoft.AspNetCore.Http;
using VDT.Core.DependencyInjection;

namespace WarOfEmpires.Services {
    [ScopedServiceImplementation(typeof(IAuthenticationService))]
    public sealed class AuthenticationService : IAuthenticationService {
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthenticationService(IHttpContextAccessor httpContextAccessor) {
            this.httpContextAccessor = httpContextAccessor;
        }

        public bool IsAuthenticated {
            get {
                return httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
            }
        }

        public string Identity {
            get {
                return httpContextAccessor.HttpContext.User.Identity.Name;
            }
        }

        public void SignIn(string identity) {
            // TODO figure out how sign in / out works
            //FormsAuthentication.SetAuthCookie(identity, false);
        }

        public void SignOut() {
            // TODO figure out how sign in / out works
            //FormsAuthentication.SignOut();
        }
    }
}