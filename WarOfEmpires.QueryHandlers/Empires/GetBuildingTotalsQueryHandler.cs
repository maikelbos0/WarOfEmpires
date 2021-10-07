using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Empires {
    [TransientServiceImplementation(typeof(IQueryHandler<GetBuildingTotalsQuery, BuildingTotalsViewModel>))]
    public sealed class GetBuildingTotalsQueryHandler : IQueryHandler<GetBuildingTotalsQuery, BuildingTotalsViewModel> {
        private readonly IReadOnlyWarContext _context;

        public GetBuildingTotalsQueryHandler(IReadOnlyWarContext context) {
            _context = context;
        }

        [Audit]
        public BuildingTotalsViewModel Execute(GetBuildingTotalsQuery query) {
            var player = _context.Players
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));
            var totalGoldSpent = player.GetTotalGoldSpentOnBuildings();

            return new BuildingTotalsViewModel() {
                TotalGoldSpent = totalGoldSpent,
                NextRecruitingLevel = Player.BuildingRecruitingLevels.Where(l => l > totalGoldSpent).Min()
            };
        }
    }
}