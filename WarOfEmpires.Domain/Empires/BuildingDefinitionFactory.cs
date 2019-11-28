using System;
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
            var costs = new ExpressionGenerator<Resources>(new Resources(gold: 20000, wood: 2000, stone: 1000, ore: 500));
            costs.Add(1, new Resources(gold: 30000, wood: 3000, stone: 1500, ore: 750));
            costs.Add(2, new Resources(gold: 50000, wood: 5000, stone: 2500, ore: 1250));
            costs.Add(3, new Resources(gold: 80000, wood: 8000, stone: 4000, ore: 2000));
            costs.Add(4, new Resources(gold: 120000, wood: 12000, stone: 12000, ore: 4000));
            costs.Add(5, new Resources(gold: 200000, wood: 15000, stone: 15000, ore: 6000));
            costs.Add(6, (int level, int levelOffset) => new Resources(
                gold: 300000 + 100000 * levelOffset,
                wood: 20000 + 5000 * levelOffset,
                stone: 20000 + 5000 * levelOffset,
                ore: 8000 + 2000 * levelOffset
            ));

            return new BuildingDefinition(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Farm (level {level})"),
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Your farm increases food production by 25% for each level; your current bonus is {level * 25}%"),
                costs
            );
        }

        private static BuildingDefinition GenerateLumberyard() {
            var costs = new ExpressionGenerator<Resources>(new Resources(gold: 20000, wood: 2000, stone: 1000, ore: 500));
            costs.Add(1, new Resources(gold: 30000, wood: 3000, stone: 1500, ore: 750));
            costs.Add(2, new Resources(gold: 50000, wood: 5000, stone: 2500, ore: 1250));
            costs.Add(3, new Resources(gold: 80000, wood: 8000, stone: 4000, ore: 2000));
            costs.Add(4, new Resources(gold: 120000, wood: 12000, stone: 12000, ore: 4000));
            costs.Add(5, new Resources(gold: 200000, wood: 15000, stone: 15000, ore: 6000));
            costs.Add(6, (int level, int levelOffset) => new Resources(
                gold: 300000 + 100000 * levelOffset,
                wood: 20000 + 5000 * levelOffset,
                stone: 20000 + 5000 * levelOffset,
                ore: 8000 + 2000 * levelOffset
            ));

            return new BuildingDefinition(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Lumberyard (level {level})"),
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Your lumberyard increases wood production by 25% for each level; your current bonus is {level * 25}%"),
                costs
            );
        }

        private static BuildingDefinition GenerateQuarry() {
            var costs = new ExpressionGenerator<Resources>(new Resources(gold: 20000, wood: 2000, stone: 1000, ore: 500));
            costs.Add(1, new Resources(gold: 30000, wood: 3000, stone: 1500, ore: 750));
            costs.Add(2, new Resources(gold: 50000, wood: 5000, stone: 2500, ore: 1250));
            costs.Add(3, new Resources(gold: 80000, wood: 8000, stone: 4000, ore: 2000));
            costs.Add(4, new Resources(gold: 120000, wood: 12000, stone: 12000, ore: 4000));
            costs.Add(5, new Resources(gold: 200000, wood: 15000, stone: 15000, ore: 6000));
            costs.Add(6, (int level, int levelOffset) => new Resources(
                gold: 300000 + 100000 * levelOffset,
                wood: 20000 + 5000 * levelOffset,
                stone: 20000 + 5000 * levelOffset,
                ore: 8000 + 2000 * levelOffset
            ));

            return new BuildingDefinition(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Quarry (level {level})"),
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Your quarry increases stone production by 25% for each level; your current bonus is {level * 25}%"),
                costs
            );
        }

        private static BuildingDefinition GenerateMine() {
            var costs = new ExpressionGenerator<Resources>(new Resources(gold: 20000, wood: 2000, stone: 1000, ore: 500));
            costs.Add(1, new Resources(gold: 30000, wood: 3000, stone: 1500, ore: 750));
            costs.Add(2, new Resources(gold: 50000, wood: 5000, stone: 2500, ore: 1250));
            costs.Add(3, new Resources(gold: 80000, wood: 8000, stone: 4000, ore: 2000));
            costs.Add(4, new Resources(gold: 120000, wood: 12000, stone: 12000, ore: 4000));
            costs.Add(5, new Resources(gold: 200000, wood: 15000, stone: 15000, ore: 6000));
            costs.Add(6, (int level, int levelOffset) => new Resources(
                gold: 300000 + 100000 * levelOffset,
                wood: 20000 + 5000 * levelOffset,
                stone: 20000 + 5000 * levelOffset,
                ore: 8000 + 2000 * levelOffset
            ));

            return new BuildingDefinition(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Mine (level {level})"),
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Your mine increases ore production by 25% for each level; your current bonus is {level * 25}%"),
                costs
            );
        }

        public static BuildingDefinition Get(BuildingType type) {
            return _buildings[type];
        }
    }
}