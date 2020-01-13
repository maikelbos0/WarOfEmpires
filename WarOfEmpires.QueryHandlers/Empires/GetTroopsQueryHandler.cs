using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Common;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetTroopsQueryHandler : IQueryHandler<GetTroopsQuery, TroopModel> {
        private readonly IWarContext _context;
        private readonly ResourcesMap _resourcesMap;

        public GetTroopsQueryHandler(IWarContext context, ResourcesMap resourcesMap) {
            _context = context;
            _resourcesMap = resourcesMap;
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
                ArcherTrainingCost = _resourcesMap.ToViewModel(Player.ArcherTrainingCost),
                CavalryTrainingCost = _resourcesMap.ToViewModel(Player.CavalryTrainingCost),
                FootmanTrainingCost = _resourcesMap.ToViewModel(Player.FootmanTrainingCost),
                MercenaryTrainingCost = _resourcesMap.ToViewModel(Player.MercenaryTrainingCost),
                WillUpkeepRunOut = !(player.GetTotalResources() + player.GetResourcesPerTurn() * 48).CanAfford(player.GetUpkeepPerTurn() * 48),
                HasUpkeepRunOut = player.HasUpkeepRunOut,
                CurrentStamina = player.Stamina,
                HasSoldierShortage = player.GetSoldierRecruitsPenalty() > 0,
                StaminaToFull = 100 - player.Stamina,
                //HealMaxAfford = ;
            };
        }
    }
}