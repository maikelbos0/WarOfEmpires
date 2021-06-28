using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Game;
using WarOfEmpires.Queries.Game;
using WarOfEmpires.QueryHandlers.Decorators;

namespace WarOfEmpires.QueryHandlers.Game {
    [TransientServiceImplementation(typeof(IQueryHandler<GetGamePhaseQuery, GamePhaseModel>))]
    public sealed class GetGamePhaseQueryHandler : IQueryHandler<GetGamePhaseQuery, GamePhaseModel> {
        private readonly IWarContext _context;

        public GetGamePhaseQueryHandler(IWarContext context) {
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
