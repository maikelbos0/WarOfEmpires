using System.Data.Entity;
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
                .Include(p => p.User)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));

            return new TaxModel() {
                Tax = player.Tax.ToString(),
                BaseGoldPerTurn = player.GetBaseGoldPerTurn(),
                BaseFoodPerTurn = player.GetBaseFoodPerTurn(),
                BaseWoodPerTurn = player.GetBaseWoodPerTurn(),
                BaseStonePerTurn = player.GetBaseStonePerTurn(),
                BaseOrePerTurn = player.GetBaseOrePerTurn(),
                CurrentGoldPerWorkerPerTurn = player.GetGoldPerWorkerPerTurn(),
                CurrentFoodPerWorkerPerTurn = player.GetFoodPerWorkerPerTurn(),
                CurrentWoodPerWorkerPerTurn = player.GetWoodPerWorkerPerTurn(),
                CurrentStonePerWorkerPerTurn = player.GetStonePerWorkerPerTurn(),
                CurrentOrePerWorkerPerTurn = player.GetOrePerWorkerPerTurn()
            };
        }
    }
}