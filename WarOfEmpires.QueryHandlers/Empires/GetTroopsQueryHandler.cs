using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetTroopsQueryHandler : IQueryHandler<GetTroopsQuery, TroopModel> {
        private readonly IWarContext _context;

        public GetTroopsQueryHandler(IWarContext context) {
            _context = context;
        }

        public TroopModel Execute(GetTroopsQuery query) {
            var player = _context.Players
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));

            return new TroopModel() {
                CurrentPeasants = player.Peasants,
                CurrentArchers = player.Archers.Soldiers,
                CurrentMercenaryArchers = player.Archers.Mercenaries,
                CurrentCavalry = player.Cavalry.Soldiers,
                CurrentMercenaryCavalry = player.Cavalry.Mercenaries,
                CurrentFootmen = player.Footmen.Soldiers,
                CurrentMercenaryFootmen = player.Footmen.Mercenaries,
                WillUpkeepRunOut = !(player.GetTotalResources() + player.GetResourcesPerTurn() * 48).CanAfford(player.GetUpkeepPerTurn() * 48),
                HasUpkeepRunOut = player.HasUpkeepRunOut,
                CurrentStamina = player.Stamina
            };
        }
    }
}
