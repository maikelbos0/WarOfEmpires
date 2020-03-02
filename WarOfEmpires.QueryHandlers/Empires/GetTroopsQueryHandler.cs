﻿using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Attacks;
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
                ArcherInfo = MapTroops(player, TroopType.Archers),
                CavalryInfo = MapTroops(player, TroopType.Cavalry),
                FootmanInfo = MapTroops(player, TroopType.Footmen),
                MercenaryTrainingCost = _resourcesMap.ToViewModel(Player.MercenaryTrainingCost),
                WillUpkeepRunOut = !(player.GetTotalResources() + player.GetResourcesPerTurn() * 48).CanAfford(player.GetUpkeepPerTurn() * 48),
                HasUpkeepRunOut = player.HasUpkeepRunOut,
                CurrentStamina = player.Stamina,
                HasSoldierShortage = player.GetSoldierRecruitsPenalty() > 0,
                StaminaToHeal = player.GetStaminaToHeal().ToString()
            };
        }

        private TroopInfoViewModel MapTroops(Player player, TroopType type) {
            var definition = TroopDefinitionFactory.Get(type);
            var troops = player.GetTroops(type);

            return new TroopInfoViewModel() {
                Cost = _resourcesMap.ToViewModel(definition.Cost),
                CurrentSoldiers = troops.Soldiers,
                CurrentMercenaries = troops.Mercenaries
            };
        }
    }
}