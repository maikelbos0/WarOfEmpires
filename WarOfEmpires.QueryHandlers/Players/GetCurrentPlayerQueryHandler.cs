using Microsoft.EntityFrameworkCore;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Players {
    [ScopedServiceImplementation(typeof(IQueryHandler<GetCurrentPlayerQuery, CurrentPlayerViewModel>))]
    public sealed class GetCurrentPlayerQueryHandler : IQueryHandler<GetCurrentPlayerQuery, CurrentPlayerViewModel> {
        private readonly IWarContext _context;

        public GetCurrentPlayerQueryHandler(IWarContext context) {
            _context = context;
        }

        [Audit]
        public CurrentPlayerViewModel Execute(GetCurrentPlayerQuery query) {
            var player = _context.Players
                .Include(p => p.User)
                .Include(p => p.AllianceRole)
                .Include(p => p.Alliance.Leader)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));

            var result = new CurrentPlayerViewModel() {
                IsAuthenticated = true,
                IsAdmin = player.User.IsAdmin,
                IsInAlliance = player.Alliance != null,
                DisplayName = player.DisplayName
            };

            if (player.Alliance != null) {
                result.IsInAlliance = true;

                if (player == player.Alliance.Leader) {
                    result.CanInvite = true;
                    result.CanManageRoles = true;
                    result.CanTransferLeadership = true;
                    result.CanDisbandAlliance = true;
                    result.CanManageNonAggressionPacts = true;
                }
                else if (player.AllianceRole != null) {
                    result.CanInvite = player.AllianceRole.CanInvite;
                    result.CanManageRoles = player.AllianceRole.CanManageRoles;
                    result.CanManageNonAggressionPacts = player.AllianceRole.CanManageNonAggressionPacts;
                    result.CanLeaveAlliance = true;
                }
            }

            return result;
        }
    }
}