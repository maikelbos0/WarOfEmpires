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
                CurrentArchers = player.Archers,
                CurrentMercenaryArchers = player.MercenaryArchers,
                CurrentCavalry = player.Cavalry,
                CurrentMercenaryCavalry = player.MercenaryCavalry,
                CurrentFootmen = player.Footmen,
                CurrentMercenaryFootmen = player.MercenaryFootmen
            };
        }
    }
}
