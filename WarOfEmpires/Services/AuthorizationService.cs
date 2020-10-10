using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.Services {
    [InterfaceInjectable]
    public class AuthorizationService : IAuthorizationService {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMessageService _messageService;

        public AuthorizationService(IAuthenticationService authenticationService, IMessageService messageService) {
            _authenticationService = authenticationService;
            _messageService = messageService;
        }

        public bool IsAdmin() {
            return _authenticationService.IsAuthenticated && _messageService.Dispatch(new GetUserIsAdminQuery(_authenticationService.Identity));
        }

        public bool IsAuthorized(IAllianceAuthorizationRequest request) {
            if (!_authenticationService.IsAuthenticated) {
                return false;
            }

            var allianceRights = _messageService.Dispatch(new GetCurrentAllianceRightsQuery(_authenticationService.Identity));

            if (!allianceRights.IsInAlliance) {
                return false;
            }

            if (request.CanInvite && !allianceRights.CanInvite) {
                return false;
            }

            if (request.CanManageRoles && !allianceRights.CanManageRoles) {
                return false;
            }

            if (request.CanDeleteChatMessages && !allianceRights.CanDeleteChatMessages) {
                return false;
            }

            return true;
        }
    }
}