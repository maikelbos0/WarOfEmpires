using System;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Common;
using WarOfEmpires.Utilities.Auditing;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Empires {
    public sealed class GetBuildingQueryHandler : IQueryHandler<GetBuildingQuery, BuildingModel> {
        private readonly IReadOnlyWarContext _context;
        private readonly IResourcesMap _resourcesMap;

        public GetBuildingQueryHandler(IReadOnlyWarContext context, IResourcesMap resourcesMap) {
            _context = context;
            _resourcesMap = resourcesMap;
        }

        [Audit]
        public BuildingModel Execute(GetBuildingQuery query) {
            var buildingType = (BuildingType)Enum.Parse(typeof(BuildingType), query.BuildingType);
            var buildingDefinition = BuildingDefinitionFactory.Get(buildingType);
            var buildingLevel = _context.Players
                .Where(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .SelectMany(p => p.Buildings)
                .SingleOrDefault(b => b.Type == buildingType)?.Level ?? 0;

            return new BuildingModel() {
                BuildingType = query.BuildingType,
                Level = buildingLevel,
                Name = buildingDefinition.GetName(buildingLevel),
                Description = buildingDefinition.GetDescription(buildingLevel),
                UpdateCost = _resourcesMap.ToViewModel(buildingDefinition.GetNextLevelCost(buildingLevel))
            };
        }
    }
}