using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Common;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Formatting;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetTroopsQueryHandler : IQueryHandler<GetTroopsQuery, TroopsModel> {
        private readonly IWarContext _context;
        private readonly ResourcesMap _resourcesMap;
        private readonly EnumFormatter _formatter;

        public GetTroopsQueryHandler(IWarContext context, ResourcesMap resourcesMap, EnumFormatter formatter) {
            _context = context;
            _resourcesMap = resourcesMap;
            _formatter = formatter;
        }

        public TroopsModel Execute(GetTroopsQuery query) {
            var player = _context.Players
                .Include(p => p.Troops)
                .Include(p => p.Workers)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));

            return new TroopsModel() {
                CurrentPeasants = player.Peasants,
                Troops = new List<TroopModel>() {
                    MapTroops(player, TroopType.Archers),
                    MapTroops(player, TroopType.Cavalry),
                    MapTroops(player, TroopType.Footmen)
                },
                MercenaryTrainingCost = _resourcesMap.ToViewModel(Player.MercenaryTrainingCost),
                WillUpkeepRunOut = player.WillUpkeepRunOut(),
                HasUpkeepRunOut = player.HasUpkeepRunOut,
                CurrentStamina = player.Stamina,
                HasSoldierShortage = player.GetSoldierRecruitsPenalty() > 0,
                StaminaToHeal = player.GetStaminaToHeal()
            };
        }

        private TroopModel MapTroops(Player player, TroopType type) {
            var definition = TroopDefinitionFactory.Get(type);
            var troops = player.GetTroops(type);

            return new TroopModel() {
                Cost = _resourcesMap.ToViewModel(definition.Cost),
                Type = type.ToString(),
                Name = _formatter.ToString(type),
                CurrentSoldiers = troops.Soldiers,
                CurrentMercenaries = troops.Mercenaries
            };
        }
    }
}