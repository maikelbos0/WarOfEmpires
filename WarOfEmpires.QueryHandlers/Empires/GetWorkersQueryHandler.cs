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
    public sealed class GetWorkersQueryHandler : IQueryHandler<GetWorkersQuery, WorkerModel> {
        private readonly IWarContext _context;

        public GetWorkersQueryHandler(IWarContext context) {
            _context = context;
        }

        public WorkerModel Execute(GetWorkersQuery query) {
            var player = _context.Players
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));
            var upkeep = player.GetUpkeepPerTurn();

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
                FoodUpkeepPerTurn = upkeep.Food,
                GoldUpkeepPerTurn = upkeep.Gold,
                RecruitsPerDay = player.GetRecruitsPerDay(),
                WillUpkeepRunOut = !(player.GetTotalResources() + player.GetResourcesPerTurn() * 48).CanAfford(player.GetUpkeepPerTurn() * 48),
                HasUpkeepRunOut = player.HasUpkeepRunOut
            };
        }
    }
}