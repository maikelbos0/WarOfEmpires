using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Empires;
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
    public sealed class GetWorkersQueryHandler : IQueryHandler<GetWorkersQuery, WorkerModel> {
        private readonly IWarContext _context;
        private readonly ResourcesMap _resourcesMap;

        public GetWorkersQueryHandler(IWarContext context, ResourcesMap resourcesMap) {
            _context = context;
            _resourcesMap = resourcesMap;
        }

        public WorkerModel Execute(GetWorkersQuery query) {
            var player = _context.Players
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));

            return new WorkerModel() {
                CurrentPeasants = player.Peasants,
                CurrentGoldPerWorkerPerTurn = player.GetGoldPerWorkerPerTurn(),
                CurrentGoldPerTurn = player.GetGoldPerTurn(),
                CurrentFarmers = player.Farmers,
                CurrentFoodPerWorkerPerTurn = player.GetFoodProduction().GetProductionPerWorker(),
                CurrentFoodPerTurn = player.GetFoodProduction().GetTotalProduction(),
                CurrentWoodWorkers = player.WoodWorkers,
                CurrentWoodPerWorkerPerTurn = player.GetWoodProduction().GetProductionPerWorker(),
                CurrentWoodPerTurn = player.GetWoodProduction().GetTotalProduction(),
                CurrentStoneMasons = player.StoneMasons,
                CurrentStonePerWorkerPerTurn = player.GetStoneProduction().GetProductionPerWorker(),
                CurrentStonePerTurn = player.GetStoneProduction().GetTotalProduction(),
                CurrentOreMiners = player.OreMiners,
                CurrentOrePerWorkerPerTurn = player.GetOreProduction().GetProductionPerWorker(),
                CurrentOrePerTurn = player.GetOreProduction().GetTotalProduction(),
                CurrentSiegeEngineers = player.SiegeEngineers,
                CurrentSiegeMaintenancePerSiegeEngineer = player.GetBuildingBonus(BuildingType.SiegeFactory),
                CurrentSiegeMaintenance = player.GetBuildingBonus(BuildingType.SiegeFactory) * player.SiegeEngineers,
                UpkeepPerTurn = _resourcesMap.ToViewModel(player.GetUpkeepPerTurn()),
                WorkerTrainingCost = _resourcesMap.ToViewModel(Player.WorkerTrainingCost),
                SiegeEngineerTrainingCost = _resourcesMap.ToViewModel(Player.SiegeEngineerTrainingCost),
                RecruitsPerDay = player.GetRecruitsPerDay(),
                WillUpkeepRunOut = !(player.GetTotalResources() + player.GetResourcesPerTurn() * 48).CanAfford(player.GetUpkeepPerTurn() * 48),
                HasUpkeepRunOut = player.HasUpkeepRunOut
            };
        }
    }
}