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
                FarmerInfo = MapWorkers(player, WorkerType.Farmers),
                WoodWorkerInfo = MapWorkers(player, WorkerType.WoodWorkers),
                StoneMasonInfo = MapWorkers(player, WorkerType.StoneMasons),
                OreMinerInfo = MapWorkers(player, WorkerType.OreMiners),
                SiegeEngineerInfo = new WorkerInfoViewModel() {
                    Cost = _resourcesMap.ToViewModel(WorkerDefinitionFactory.Get(WorkerType.SiegeEngineers).Cost),
                    CurrentWorkers = player.GetWorkerCount(WorkerType.SiegeEngineers),
                    CurrentProductionPerWorkerPerTurn = player.GetBuildingBonus(BuildingType.SiegeFactory),
                    CurrentProductionPerTurn = player.GetBuildingBonus(BuildingType.SiegeFactory) * player.GetWorkerCount(WorkerType.SiegeEngineers)
                },
                MerchantInfo = new WorkerInfoViewModel() {
                    Cost = _resourcesMap.ToViewModel(WorkerDefinitionFactory.Get(WorkerType.Merchants).Cost),
                    CurrentWorkers = player.GetWorkerCount(WorkerType.Merchants),
                    CurrentProductionPerWorkerPerTurn = player.GetBuildingBonus(BuildingType.Market),
                    CurrentProductionPerTurn = player.GetBuildingBonus(BuildingType.Market) * player.GetWorkerCount(WorkerType.Merchants)
                },
                UpkeepPerTurn = _resourcesMap.ToViewModel(player.GetUpkeepPerTurn()),
                RecruitsPerDay = player.GetRecruitsPerDay(),
                WillUpkeepRunOut = !(player.GetTotalResources() + player.GetResourcesPerTurn() * 48).CanAfford(player.GetUpkeepPerTurn() * 48),
                HasUpkeepRunOut = player.HasUpkeepRunOut
            };
        }

        private WorkerInfoViewModel MapWorkers(Player player, WorkerType type) {
            var definition = WorkerDefinitionFactory.Get(type);
            var productionInfo = player.GetProduction(type);

            return new WorkerInfoViewModel() {
                Cost = _resourcesMap.ToViewModel(definition.Cost),
                CurrentWorkers = player.GetWorkerCount(type),
                CurrentProductionPerWorkerPerTurn = productionInfo.GetProductionPerWorker(),
                CurrentProductionPerTurn = productionInfo.GetTotalProduction()
            };
        }
    }
}