using System.Collections.Generic;
using WarOfEmpires.Domain.Common;

namespace WarOfEmpires.Domain.Empires {
    public static class BuildingDefinitionFactory {
        private static readonly Dictionary<BuildingType, BuildingDefinition> _buildings = new Dictionary<BuildingType, BuildingDefinition>();

        static BuildingDefinitionFactory() {
            _buildings.Add(BuildingType.Farm, GenerateFarm());
            _buildings.Add(BuildingType.Lumberyard, GenerateLumberyard());
            _buildings.Add(BuildingType.Quarry, GenerateQuarry());
            _buildings.Add(BuildingType.Mine, GenerateMine());
        }

        private static BuildingDefinition GenerateFarm() {
            var farmCosts = new ExpressionGenerator<Resources>(new Resources(gold: 20000, wood: 2000, stone: 1000, ore: 500));
            farmCosts.Add(1, new Resources(gold: 50000, wood: 5000, stone: 2000, ore: 1000));
            farmCosts.Add(2, new Resources(gold: 100000, wood: 10000, stone: 5000, ore: 2000));
            farmCosts.Add(2, new Resources(gold: 150000, wood: 15000, stone: 8000, ore: 5000));

            return new BuildingDefinition(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Farm (level {level})"),
                farmCosts
            );
        }

        private static BuildingDefinition GenerateLumberyard() {
            var farmCosts = new ExpressionGenerator<Resources>(new Resources(gold: 20000, wood: 2000, stone: 1000, ore: 500));
            farmCosts.Add(1, new Resources(gold: 50000, wood: 5000, stone: 2000, ore: 1000));
            farmCosts.Add(2, new Resources(gold: 100000, wood: 10000, stone: 5000, ore: 2000));
            farmCosts.Add(2, new Resources(gold: 150000, wood: 15000, stone: 8000, ore: 5000));

            return new BuildingDefinition(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Lumberyard (level {level})"),
                farmCosts
            );
        }

        private static BuildingDefinition GenerateQuarry() {
            var farmCosts = new ExpressionGenerator<Resources>(new Resources(gold: 20000, wood: 2000, stone: 1000, ore: 500));
            farmCosts.Add(1, new Resources(gold: 50000, wood: 5000, stone: 2000, ore: 1000));
            farmCosts.Add(2, new Resources(gold: 100000, wood: 10000, stone: 5000, ore: 2000));
            farmCosts.Add(2, new Resources(gold: 150000, wood: 15000, stone: 8000, ore: 5000));

            return new BuildingDefinition(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Quarry (level {level})"),
                farmCosts
            );
        }

        private static BuildingDefinition GenerateMine() {
            var farmCosts = new ExpressionGenerator<Resources>(new Resources(gold: 20000, wood: 2000, stone: 1000, ore: 500));
            farmCosts.Add(1, new Resources(gold: 50000, wood: 5000, stone: 2000, ore: 1000));
            farmCosts.Add(2, new Resources(gold: 100000, wood: 10000, stone: 5000, ore: 2000));
            farmCosts.Add(2, new Resources(gold: 150000, wood: 15000, stone: 8000, ore: 5000));

            return new BuildingDefinition(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Mine (level {level})"),
                farmCosts
            );
        }

        public static BuildingDefinition Get(BuildingType type) {
            return _buildings[type];
        }
    }
}