using Microsoft.EntityFrameworkCore;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Players {
    public sealed class GetCurrentPlayerQueryHandler : IQueryHandler<GetCurrentPlayerQuery, CurrentPlayerViewModel> {
        private readonly IReadOnlyWarContext _context;

        public GetCurrentPlayerQueryHandler(IReadOnlyWarContext context) {
            _context = context;
        }

        [Audit]
        public CurrentPlayerViewModel Execute(GetCurrentPlayerQuery query) {
            var player = _context.Players
                .Include(p => p.User)
                .Include(p => p.AllianceRole)
                .Include(p => p.Alliance.Leader)
                .SingleOrDefault(p => EmailComparisonService.Equals(p.User.Email, query.Email));

            var result = new CurrentPlayerViewModel() {
                IsAuthenticated = true,
                IsAdmin = player?.User.IsAdmin ?? false,
                IsPlayer = player != null,
                IsInAlliance = player?.Alliance != null,
                DisplayName = player?.DisplayName
            };

            if (player?.Alliance != null) {
                result.IsInAlliance = true;

                if (player == player.Alliance.Leader) {
                    result.CanInvite = true;
                    result.CanManageRoles = true;
                    result.CanTransferLeadership = true;
                    result.CanDisbandAlliance = true;
                    result.CanManageNonAggressionPacts = true;
                    result.CanManageWars = true;
                    result.CanBank = true;
                }
                else {
                    result.CanLeaveAlliance = true;

                    if (player.AllianceRole != null) {
                        result.CanInvite = player.AllianceRole.CanInvite;
                        result.CanManageRoles = player.AllianceRole.CanManageRoles;
                        result.CanManageNonAggressionPacts = player.AllianceRole.CanManageNonAggressionPacts;
                        result.CanManageWars = player.AllianceRole.CanManageWars;
                        result.CanBank = player.AllianceRole.CanBank;
                    }
                }
            }

            return result;
        }
    }
}
