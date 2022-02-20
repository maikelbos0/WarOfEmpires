using Microsoft.EntityFrameworkCore;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Alliances {
    public sealed class GetCurrentAllianceRightsQueryHandler : IQueryHandler<GetCurrentAllianceRightsQuery, CurrentAllianceRightsViewModel> {
        private readonly IReadOnlyWarContext _context;

        public GetCurrentAllianceRightsQueryHandler(IReadOnlyWarContext context) {
            _context = context;
        }

        [Audit]
        public CurrentAllianceRightsViewModel Execute(GetCurrentAllianceRightsQuery query) {
            var player = _context.Players
                .Include(p => p.Alliance.Leader)
                .Include(p => p.AllianceRole)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));

            var result = new CurrentAllianceRightsViewModel() {
                IsInAlliance = player.Alliance != null
            };

            if (player == player.Alliance?.Leader) {
                result.CanInvite = true;
                result.CanManageRoles = true;
                result.CanDeleteChatMessages = true;
                result.CanKickMembers = true;
                result.CanTransferLeadership = true;
                result.CanDisbandAlliance = true;
                result.CanManageNonAggressionPacts = true;
                result.CanManageWars = true;
                result.CanBank = true;
            }
            else if (player.AllianceRole != null) {
                result.CanInvite = player.AllianceRole.CanInvite;
                result.CanManageRoles = player.AllianceRole.CanManageRoles;
                result.CanDeleteChatMessages = player.AllianceRole.CanDeleteChatMessages;
                result.CanKickMembers = player.AllianceRole.CanKickMembers;
                result.CanManageNonAggressionPacts = player.AllianceRole.CanManageNonAggressionPacts;
                result.CanManageWars = player.AllianceRole.CanManageWars;
                result.CanBank = player.AllianceRole.CanBank;
            }

            return result;
        }
    }
}