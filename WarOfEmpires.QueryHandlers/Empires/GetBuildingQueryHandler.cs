using System;
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
    public sealed class GetBuildingQueryHandler : IQueryHandler<GetBuildingQuery, BuildingViewModel> {
        private readonly IWarContext _context;
        private readonly ResourcesMap _resourcesMap;

        public GetBuildingQueryHandler(IWarContext context, ResourcesMap resourcesMap) {
            _context = context;
            _resourcesMap = resourcesMap;
        }

        public BuildingViewModel Execute(GetBuildingQuery query) {
            var buildingType = (BuildingType)Enum.Parse(typeof(BuildingType), query.BuildingType);
            var buildingDefinition = BuildingDefinitionFactory.Get(buildingType);
            var buildingLevel = _context.Players
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .Buildings.SingleOrDefault(b => b.Type == buildingType)?.Level ?? 0;

            return new BuildingViewModel() {
                Level = buildingLevel,
                Name = buildingDefinition.GetName(buildingLevel),
                UpdateCost = _resourcesMap.ToViewModel(buildingDefinition.GetNextLevelCost(buildingLevel))
            };
        }
    }
}