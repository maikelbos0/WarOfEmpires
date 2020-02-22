using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Players {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetPlayerDetailsQueryHandler : IQueryHandler<GetPlayerDetailsQuery, PlayerDetailsViewModel> {
        private readonly IWarContext _context;

        public GetPlayerDetailsQueryHandler(IWarContext context) {
            _context = context;
        }

        public PlayerDetailsViewModel Execute(GetPlayerDetailsQuery query) {
            var id = int.Parse(query.Id);
            var player = _context.Players
                .Single(p => p.User.Status == UserStatus.Active && p.Id == id);

            return new PlayerDetailsViewModel() {
                Id = player.Id,
                DisplayName = player.DisplayName,
                Population = player.Farmers + player.WoodWorkers + player.StoneMasons + player.OreMiners + player.Peasants + player.SiegeEngineers
                    + player.Troops.Sum(t => t.GetTotals())
            };
        }
    }
}