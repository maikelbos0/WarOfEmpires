using WarOfEmpires.Utilities.Container;
using AlloyTemplates;

namespace WarOfEmpires.Services {
    [InterfaceInjectable]
    public sealed class AuthenticationService : IAuthenticationService {
        public bool IsAuthenticated {
            get {
                return HttpContextHelper.Current.User.Identity.IsAuthenticated;
            }
        }

        public string Identity {
            get {
                return HttpContextHelper.Current.User.Identity.Name;
            }
        }

        public void SignIn(string identity) {
            FormsAuthentication.SetAuthCookie(identity, false);
        }

        public void SignOut() {
            FormsAuthentication.SignOut();
        }
    }
}