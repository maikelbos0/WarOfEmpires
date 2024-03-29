﻿using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Empires {
    public sealed class GetBuildingTotalsQueryHandler : IQueryHandler<GetBuildingTotalsQuery, BuildingTotalsViewModel> {
        private readonly IReadOnlyWarContext _context;

        public GetBuildingTotalsQueryHandler(IReadOnlyWarContext context) {
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