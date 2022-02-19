using System.Collections.Generic;
using System.Linq;
using VDT.Core.DependencyInjection.Attributes;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Players {
    [TransientServiceImplementation(typeof(IQueryHandler<GetBlockedPlayersQuery, IEnumerable<BlockedPlayerViewModel>>))]
    public sealed class GetBlockedPlayersQueryHandler : IQueryHandler<GetBlockedPlayersQuery, IEnumerable<BlockedPlayerViewModel>> {
        private readonly IReadOnlyWarContext _context;

        public GetBlockedPlayersQueryHandler(IReadOnlyWarContext context) {
            _context = context;
        }

        [Audit]
        public IEnumerable<BlockedPlayerViewModel> Execute(GetBlockedPlayersQuery query) {
            return _context.Players
                .Where(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .SelectMany(p => p.PlayerBlocks)
                .Select(b => b.BlockedPlayer)
                .Where(p => p.User.Status == UserStatus.Active)
                .Select(p => new BlockedPlayerViewModel() {
                    Id = p.Id,
                    DisplayName = p.DisplayName,
                    Alliance = p.Alliance == null ? null : p.Alliance.Code
                });
        }
    }
}
