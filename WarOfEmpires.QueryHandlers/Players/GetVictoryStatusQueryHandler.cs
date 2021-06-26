using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Decorators;

namespace WarOfEmpires.QueryHandlers.Players {
    [TransientServiceImplementation(typeof(IQueryHandler<GetVictoryStatusQuery, VictoryStatusViewModel>))]
    public sealed class GetVictoryStatusQueryHandler : IQueryHandler<GetVictoryStatusQuery, VictoryStatusViewModel> {
        private readonly IWarContext _context;

        public GetVictoryStatusQueryHandler(IWarContext context) {
            _context = context;
        }

        [Audit]
        public VictoryStatusViewModel Execute(GetVictoryStatusQuery query) {
            throw new System.NotImplementedException();
        }
    }
}
