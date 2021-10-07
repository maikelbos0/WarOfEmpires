using Microsoft.EntityFrameworkCore;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Empires {
    [TransientServiceImplementation(typeof(IQueryHandler<GetHousingTotalsQuery, HousingTotalsViewModel>))]
    public sealed class GetHousingTotalsQueryHandler : IQueryHandler<GetHousingTotalsQuery, HousingTotalsViewModel> {
        private readonly IReadOnlyWarContext _context;

        public GetHousingTotalsQueryHandler(IReadOnlyWarContext context) {
            _context = context;
        }

        [Audit]
        public HousingTotalsViewModel Execute(GetHousingTotalsQuery query) {
            var player = _context.Players
                .Include(p => p.Buildings)
                .Include(p => p.Workers)
                .Include(p => p.Troops)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));

            return new HousingTotalsViewModel() {
                BarracksCapacity = player.GetBuildingBonus(BuildingType.Barracks),
                BarracksOccupancy = player.Troops.Sum(t => t.GetTotals()),
                HutCapacity = player.GetBuildingBonus(BuildingType.Huts),
                HutOccupancy = player.Workers.Sum(w => w.Count),
                TotalCapacity = player.GetBuildingBonus(BuildingType.Barracks) + player.GetBuildingBonus(BuildingType.Huts),
                TotalOccupancy = player.Troops.Sum(t => t.GetTotals()) + player.Workers.Sum(w => w.Count) + player.Peasants,
                HasHousingShortage = player.GetTheoreticalRecruitsPerDay() > player.GetAvailableHousingCapacity()
            };
        }
    }
}