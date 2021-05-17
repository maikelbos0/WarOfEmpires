using Microsoft.EntityFrameworkCore;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Empires {
    [TransientServiceImplementation(typeof(IQueryHandler<GetTaxQuery, TaxModel>))]
    public sealed class GetTaxQueryHandler : IQueryHandler<GetTaxQuery, TaxModel> {
        private readonly IWarContext _context;

        public GetTaxQueryHandler(IWarContext context) {
            _context = context;
        }

        [Audit]
        public TaxModel Execute(GetTaxQuery query) {
            var player = _context.Players
                .Include(p => p.Buildings)
                .Include(p => p.Workers)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));

            return new TaxModel() {
                Tax = player.Tax,
                BaseGoldPerTurn = Player.BaseGoldPerTurn,
                BaseFoodPerTurn = player.GetProduction(WorkerType.Farmers).GetBaseProduction(),
                BaseWoodPerTurn = player.GetProduction(WorkerType.WoodWorkers).GetBaseProduction(),
                BaseStonePerTurn = player.GetProduction(WorkerType.StoneMasons).GetBaseProduction(),
                BaseOrePerTurn = player.GetProduction(WorkerType.OreMiners).GetBaseProduction(),
                CurrentGoldPerWorkerPerTurn = player.GetGoldPerWorkerPerTurn(),
                CurrentFoodPerWorkerPerTurn = player.GetProduction(WorkerType.Farmers).GetProductionPerWorker(),
                CurrentWoodPerWorkerPerTurn = player.GetProduction(WorkerType.WoodWorkers).GetProductionPerWorker(),
                CurrentStonePerWorkerPerTurn = player.GetProduction(WorkerType.StoneMasons).GetProductionPerWorker(),
                CurrentOrePerWorkerPerTurn = player.GetProduction(WorkerType.OreMiners).GetProductionPerWorker()
            };
        }
    }
}