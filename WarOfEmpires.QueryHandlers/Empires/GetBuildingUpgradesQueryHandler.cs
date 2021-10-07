using System;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Common;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Empires {
    [TransientServiceImplementation(typeof(IQueryHandler<GetBuildingUpgradesQuery, BuildingUpgradesViewModel>))]
    public sealed class GetBuildingUpgradesQueryHandler : IQueryHandler<GetBuildingUpgradesQuery, BuildingUpgradesViewModel> {
        private readonly IReadOnlyWarContext _context;
        private readonly IResourcesMap _resourcesMap;

        public GetBuildingUpgradesQueryHandler(IReadOnlyWarContext context, IResourcesMap resourcesMap) {
            _context = context;
            _resourcesMap = resourcesMap;
        }

        [Audit]
        public BuildingUpgradesViewModel Execute(GetBuildingUpgradesQuery query) {
            var buildingType = (BuildingType)Enum.Parse(typeof(BuildingType), query.BuildingType);
            var buildingDefinition = BuildingDefinitionFactory.Get(buildingType);
            var buildingLevel = _context.Players
                .Where(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .SelectMany(p => p.Buildings)
                .SingleOrDefault(b => b.Type == buildingType)?.Level ?? 0;

            return new BuildingUpgradesViewModel() {
                Name = buildingDefinition.GetName(buildingLevel),
                Upgrades = Enumerable.Range(buildingLevel, 15)
                    .Select(l => new BuildingUpgradeViewModel() {
                        Name = buildingDefinition.GetName(l + 1),
                        Resources = _resourcesMap.ToViewModel(buildingDefinition.GetNextLevelCost(l)),
                        Bonus = buildingDefinition.GetBonus(l + 1)
                    })
                    .ToList()
            };
        }
    }
}
