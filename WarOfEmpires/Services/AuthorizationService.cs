using VDT.Core.DependencyInjection.Attributes;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.Queries.Security;

namespace WarOfEmpires.Services {
    [ScopedServiceImplementation(typeof(IAuthorizationService))]
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

            if (request.CanKickMembers && !allianceRights.CanKickMembers) {
                return false;
            }

            if (request.CanTransferLeadership && !allianceRights.CanTransferLeadership) {
                return false;
            }

            if (request.CanDisbandAlliance && !allianceRights.CanDisbandAlliance) {
                return false;
            }

            if (request.CanManageNonAggressionPacts && !allianceRights.CanManageNonAggressionPacts) {
                return false;
            }

            if (request.CanManageWars && !allianceRights.CanManageWars) {
                return false;
            }

            if (request.CanBank && !allianceRights.CanBank) {
                return false;
            }

            return true;
        }
    }
}