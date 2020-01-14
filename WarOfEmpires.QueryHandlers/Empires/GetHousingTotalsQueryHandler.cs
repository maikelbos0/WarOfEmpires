using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetHousingTotalsQueryHandler : IQueryHandler<GetHousingTotalsQuery, HousingTotalsViewModel> {
        private readonly IWarContext _context;

        public GetHousingTotalsQueryHandler(IWarContext context) {
            _context = context;
        }

        public HousingTotalsViewModel Execute(GetHousingTotalsQuery query) {
            var player = _context.Players
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));

            return new HousingTotalsViewModel() {
                BarracksCapacity = player.Buildings.SingleOrDefault(b => b.Type == BuildingType.Barracks).Level * 10,
                BarracksOccupancy = player.Archers.GetTotals() + player.Cavalry.GetTotals() + player.Footmen.GetTotals(),
                HutCapacity = player.Buildings.SingleOrDefault(b => b.Type == BuildingType.Huts).Level * 10,
                HutOccupancy = player.Peasants + player.Farmers + player.WoodWorkers + player.StoneMasons + player.OreMiners + player.SiegeEngineers,
                HasHousingShortage = player.GetTheoreticalRecruitsPerDay() > player.GetAvailableHousingCapacity()
            };
        }
    }
}