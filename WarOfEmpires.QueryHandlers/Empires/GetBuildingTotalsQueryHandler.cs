using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetBuildingTotalsQueryHandler : IQueryHandler<GetBuildingTotalsQuery, BuildingTotalsViewModel> {
        private readonly IWarContext _context;

        public GetBuildingTotalsQueryHandler(IWarContext context) {
            _context = context;
        }

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