using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Game;
using WarOfEmpires.Queries.Game;
using WarOfEmpires.QueryHandlers.Decorators;

namespace WarOfEmpires.QueryHandlers.Game {
    public sealed class GetGamePhaseQueryHandler : IQueryHandler<GetGamePhaseQuery, GamePhaseModel> {
        private readonly IReadOnlyWarContext _context;

        public GetGamePhaseQueryHandler(IReadOnlyWarContext context) {
            _context = context;
        }

        [Audit]
        public GamePhaseModel Execute(GetGamePhaseQuery query) {
            return _context.GameStatus.Select(s => new GamePhaseModel() {
                Phase = s.Phase.ToString()
            }).Single();
        }
    }
}
