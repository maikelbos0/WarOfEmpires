using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Siege;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Common;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public class GetSiegeQueryHandler : IQueryHandler<GetSiegeQuery, SiegeModel> {
        private readonly IWarContext _context;
        private readonly ResourcesMap _resourcesMap;

        public GetSiegeQueryHandler(IWarContext context, ResourcesMap resourcesMap) {
            _context = context;
            _resourcesMap = resourcesMap;
        }

        public SiegeModel Execute(GetSiegeQuery query) {
            var player = _context.Players
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));
            
            return new SiegeModel() {
                Engineers = player.GetWorkerCount(WorkerType.SiegeEngineers),
                TotalMaintenance = player.GetWorkerCount(WorkerType.SiegeEngineers) * player.GetBuildingBonus(BuildingType.SiegeFactory),
                AvailableMaintenance = player.GetWorkerCount(WorkerType.SiegeEngineers) * player.GetBuildingBonus(BuildingType.SiegeFactory)
                    - player.SiegeWeapons.Sum(s => s.Count * SiegeWeaponDefinitionFactory.Get(s.Type).Maintenance),
                SiegeWeapons = new List<SiegeWeaponModel>() {
                    MapSiegeWeapon(player, SiegeWeaponType.FireArrows),
                    MapSiegeWeapon(player, SiegeWeaponType.BatteringRams),
                    MapSiegeWeapon(player, SiegeWeaponType.ScalingLadders)
                }
            };
        }

        private SiegeWeaponModel MapSiegeWeapon(Player player, SiegeWeaponType type) {
            var definition = SiegeWeaponDefinitionFactory.Get(type);
            var count = player.GetSiegeWeaponCount(type);

            return new SiegeWeaponModel() {
                Type = definition.Type.ToString(),
                Name = definition.Name,
                Description = definition.Description,
                Maintenance = definition.Maintenance,
                TroopCount = definition.TroopCount,
                Cost = _resourcesMap.ToViewModel(definition.Cost),
                CurrentCount = count,
                CurrentTroopCount = definition.TroopCount * count
            };
        }
    }
}