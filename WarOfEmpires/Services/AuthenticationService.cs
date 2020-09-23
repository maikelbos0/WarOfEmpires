using System.Web;
using System.Web.Security;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.Services {
    [InterfaceInjectable]
    public sealed class AuthenticationService : IAuthenticationService {
        private readonly IMessageService _messageService;

        public AuthenticationService(IMessageService messageService) {
            _messageService = messageService;
        }

        public bool IsAuthenticated {
            get {
                return HttpContext.Current.User.Identity.IsAuthenticated;
            }
        }

        public string Identity {
            get {
                return HttpContext.Current.User.Identity.Name;
            }
        }

        public bool IsAdmin() {
            return IsAuthenticated && _messageService.Dispatch(new GetUserIsAdminQuery(Identity));
        }

        public CurrentAllianceRightsViewModel GetAllianceRights() {
            return _messageService.Dispatch(new GetCurrentAllianceRightsQuery(Identity));
        }

        public void SignIn(string identity) {
            FormsAuthentication.SetAuthCookie(identity, false);
        }

        public void SignOut() {
            FormsAuthentication.SignOut();
        }
    }
}