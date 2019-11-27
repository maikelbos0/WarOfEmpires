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

            return new WorkerModel() {
                CurrentPeasants = player.Peasants,
                CurrentGoldPerWorkerPerTurn = player.GetGoldPerWorkerPerTurn(),
                CurrentGoldPerTurn = player.GetGoldPerTurn(),
                CurrentFarmers = player.Farmers,
                CurrentFoodPerWorkerPerTurn = player.GetFoodPerWorkerPerTurn(),
                CurrentFoodPerTurn = player.GetFoodPerTurn(),
                CurrentWoodWorkers = player.WoodWorkers,
                CurrentWoodPerWorkerPerTurn = player.GetWoodPerWorkerPerTurn(),
                CurrentWoodPerTurn = player.GetWoodPerTurn(),
                CurrentStoneMasons = player.StoneMasons,
                CurrentStonePerWorkerPerTurn = player.GetStonePerWorkerPerTurn(),
                CurrentStonePerTurn = player.GetStonePerTurn(),
                CurrentOreMiners = player.OreMiners,
                CurrentOrePerWorkerPerTurn = player.GetOrePerWorkerPerTurn(),
                CurrentOrePerTurn = player.GetOrePerTurn()
            };
        }
    }
}