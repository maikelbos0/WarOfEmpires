using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Attacks;
using WarOfEmpires.Queries.Attacks;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Attacks {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetDefenderQueryHandler : IQueryHandler<GetDefenderQuery, ExecuteAttackModel> {
        private readonly IWarContext _context;

        public GetDefenderQueryHandler(IWarContext context) {
            _context = context;
        }

        public ExecuteAttackModel Execute(GetDefenderQuery query) {
            var id = int.Parse(query.Id);
            var player = _context.Players
                .Single(p => p.User.Status == UserStatus.Active && p.Id == id);
            
            return new ExecuteAttackModel() {
                DefenderId = player.Id,
                DisplayName = player.DisplayName,
                Population = player.Farmers + player.WoodWorkers + player.StoneMasons + player.OreMiners + player.Peasants + player.SiegeEngineers
                    + player.Troops.Sum(t => t.GetTotals())
            };
        }
    }
}
