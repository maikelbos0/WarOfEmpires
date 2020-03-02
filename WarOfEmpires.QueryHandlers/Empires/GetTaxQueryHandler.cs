using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetTaxQueryHandler : IQueryHandler<GetTaxQuery, TaxModel> {
        private readonly IWarContext _context;

        public GetTaxQueryHandler(IWarContext context) {
            _context = context;
        }

        public TaxModel Execute(GetTaxQuery query) {
            var player = _context.Players
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));

            return new TaxModel() {
                Tax = player.Tax.ToString(),
                BaseGoldPerTurn = Player.BaseGoldPerTurn,
                BaseFoodPerTurn = player.GetProduction(WorkerType.Farmer).GetBaseProduction(),
                BaseWoodPerTurn = player.GetProduction(WorkerType.WoodWorker).GetBaseProduction(),
                BaseStonePerTurn = player.GetProduction(WorkerType.StoneMason).GetBaseProduction(),
                BaseOrePerTurn = player.GetProduction(WorkerType.OreMiner).GetBaseProduction(),
                CurrentGoldPerWorkerPerTurn = player.GetGoldPerWorkerPerTurn(),
                CurrentFoodPerWorkerPerTurn = player.GetProduction(WorkerType.Farmer).GetProductionPerWorker(),
                CurrentWoodPerWorkerPerTurn = player.GetProduction(WorkerType.WoodWorker).GetProductionPerWorker(),
                CurrentStonePerWorkerPerTurn = player.GetProduction(WorkerType.StoneMason).GetProductionPerWorker(),
                CurrentOrePerWorkerPerTurn = player.GetProduction(WorkerType.OreMiner).GetProductionPerWorker()
            };
        }
    }
}