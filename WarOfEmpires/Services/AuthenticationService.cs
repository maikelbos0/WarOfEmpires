using WarOfEmpires.Queries.Security;
using WarOfEmpires.Utilities.Container;
using System;
using System.Web;
using System.Web.Security;

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

        public string GetUserName() {
            if (!IsAuthenticated) {
                throw new InvalidOperationException("User is not authenticated");
            }

            return _messageService.Dispatch(new GetUserNameQuery(Identity));
        }

        public void SignIn(string identity) {
            FormsAuthentication.SetAuthCookie(identity, false);
        }

        public void SignOut() {
            FormsAuthentication.SignOut();
        }
    }
}