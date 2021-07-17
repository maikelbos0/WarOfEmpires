using System.Collections.Generic;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Decorators;

namespace WarOfEmpires.QueryHandlers.Players {
    [TransientServiceImplementation(typeof(IQueryHandler<GetBlockedPlayersQuery, IEnumerable<BlockedPlayerViewModel>>))]
    public sealed class GetBlockedPlayersQueryHandler : IQueryHandler<GetBlockedPlayersQuery, IEnumerable<BlockedPlayerViewModel>> {
        private readonly IWarContext _context;

        public GetBlockedPlayersQueryHandler(IWarContext context) {
            _context = context;
        }

        [Audit]
        public IEnumerable<BlockedPlayerViewModel> Execute(GetBlockedPlayersQuery query) {
            throw new System.NotImplementedException();
        }
    }
}
