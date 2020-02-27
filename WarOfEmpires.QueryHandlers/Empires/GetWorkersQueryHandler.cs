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

            // TODO change the current worker information to a separate model
            return new WorkerModel() {
                CurrentPeasants = player.Peasants,
                CurrentGoldPerWorkerPerTurn = player.GetGoldPerWorkerPerTurn(),
                CurrentGoldPerTurn = player.GetGoldPerTurn(),
                CurrentFarmers = player.GetWorkerCount(WorkerType.Farmer),
                CurrentFoodPerWorkerPerTurn = player.GetProduction(WorkerType.Farmer).GetProductionPerWorker(),
                CurrentFoodPerTurn = player.GetProduction(WorkerType.Farmer).GetTotalProduction(),
                CurrentWoodWorkers = player.GetWorkerCount(WorkerType.WoodWorker),
                CurrentWoodPerWorkerPerTurn = player.GetProduction(WorkerType.WoodWorker).GetProductionPerWorker(),
                CurrentWoodPerTurn = player.GetProduction(WorkerType.WoodWorker).GetTotalProduction(),
                CurrentStoneMasons = player.GetWorkerCount(WorkerType.StoneMason),
                CurrentStonePerWorkerPerTurn = player.GetProduction(WorkerType.StoneMason).GetProductionPerWorker(),
                CurrentStonePerTurn = player.GetProduction(WorkerType.StoneMason).GetTotalProduction(),
                CurrentOreMiners = player.GetWorkerCount(WorkerType.OreMiner),
                CurrentOrePerWorkerPerTurn = player.GetProduction(WorkerType.OreMiner).GetProductionPerWorker(),
                CurrentOrePerTurn = player.GetProduction(WorkerType.OreMiner).GetTotalProduction(),
                CurrentSiegeEngineers = player.GetWorkerCount(WorkerType.SiegeEngineer),
                CurrentSiegeMaintenancePerSiegeEngineer = player.GetBuildingBonus(BuildingType.SiegeFactory),
                CurrentSiegeMaintenance = player.GetBuildingBonus(BuildingType.SiegeFactory) * player.GetWorkerCount(WorkerType.SiegeEngineer),
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