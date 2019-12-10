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
                BaseGoldPerTurn = player.GetBaseGoldPerTurn(),
                BaseFoodPerTurn = player.GetFoodProduction().GetBaseProduction(),
                BaseWoodPerTurn = player.GetWoodProduction().GetBaseProduction(),
                BaseStonePerTurn = player.GetStoneProduction().GetBaseProduction(),
                BaseOrePerTurn = player.GetOreProduction().GetBaseProduction(),
                CurrentGoldPerWorkerPerTurn = player.GetGoldPerWorkerPerTurn(),
                CurrentFoodPerWorkerPerTurn = player.GetFoodProduction().GetProductionPerWorker(),
                CurrentWoodPerWorkerPerTurn = player.GetWoodProduction().GetProductionPerWorker(),
                CurrentStonePerWorkerPerTurn = player.GetStoneProduction().GetProductionPerWorker(),
                CurrentOrePerWorkerPerTurn = player.GetOreProduction().GetProductionPerWorker()
            };
        }
    }
}