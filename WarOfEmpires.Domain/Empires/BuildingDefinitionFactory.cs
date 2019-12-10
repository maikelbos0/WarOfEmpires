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
            _buildings.Add(BuildingType.Forge, GenerateForge());
            _buildings.Add(BuildingType.Armoury, GenerateArmoury());
            _buildings.Add(BuildingType.ArcheryRange, GenerateArcheryRange());
            _buildings.Add(BuildingType.CavalryRange, GenerateCavalryRange());
            _buildings.Add(BuildingType.FootmanRange, GenerateFootmanRange());
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

        private static BuildingDefinition GenerateForge() {
            var costs = new ExpressionGenerator<Resources>(new Resources(gold: 20000, wood: 1000, stone: 500, ore: 2000));
            costs.Add(1, new Resources(gold: 30000, wood: 1500, stone: 750, ore: 3000));
            costs.Add(2, new Resources(gold: 50000, wood: 2500, stone: 1250, ore: 5000));
            costs.Add(3, new Resources(gold: 80000, wood: 4000, stone: 2000, ore: 8000));
            costs.Add(4, new Resources(gold: 120000, wood: 5000, stone: 5000, ore: 15000));
            costs.Add(5, new Resources(gold: 200000, wood: 8000, stone: 8000, ore: 20000));
            costs.Add(6, (int level, int levelOffset) => new Resources(
                gold: 300000 + 100000 * levelOffset,
                wood: 10000 + 2500 * levelOffset,
                stone: 10000 + 2500 * levelOffset,
                ore: 32000 + 8000 * levelOffset
            ));

            return new BuildingDefinition(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Forge (level {level})"),
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Your forge increases your troops' attack strength by 25% for each level; your current bonus is {level * 25}%"),
                costs
            );
        }

        private static BuildingDefinition GenerateArmoury() {
            var costs = new ExpressionGenerator<Resources>(new Resources(gold: 20000, wood: 1000, stone: 500, ore: 2000));
            costs.Add(1, new Resources(gold: 30000, wood: 1500, stone: 750, ore: 3000));
            costs.Add(2, new Resources(gold: 50000, wood: 2500, stone: 1250, ore: 5000));
            costs.Add(3, new Resources(gold: 80000, wood: 4000, stone: 2000, ore: 8000));
            costs.Add(4, new Resources(gold: 120000, wood: 5000, stone: 5000, ore: 15000));
            costs.Add(5, new Resources(gold: 200000, wood: 8000, stone: 8000, ore: 20000));
            costs.Add(6, (int level, int levelOffset) => new Resources(
                gold: 300000 + 100000 * levelOffset,
                wood: 10000 + 2500 * levelOffset,
                stone: 10000 + 2500 * levelOffset,
                ore: 32000 + 8000 * levelOffset
            ));

            return new BuildingDefinition(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Armoury (level {level})"),
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Your armoury increases your troops' defensive strength by 25% for each level; your current bonus is {level * 25}%"),
                costs
            );
        }

        private static BuildingDefinition GenerateArcheryRange() {
            var costs = new ExpressionGenerator<Resources>(new Resources(gold: 20000, wood: 500, stone: 2000, ore: 1000));
            costs.Add(1, new Resources(gold: 30000, wood: 750, stone: 3000, ore: 1500));
            costs.Add(2, new Resources(gold: 50000, wood: 1250, stone: 5000, ore: 2500));
            costs.Add(3, new Resources(gold: 80000, wood: 2000, stone: 8000, ore: 4000));
            costs.Add(4, new Resources(gold: 120000, wood: 6000, stone: 12000, ore: 6000));
            costs.Add(5, new Resources(gold: 200000, wood: 10000, stone: 15000, ore: 10000));
            costs.Add(6, (int level, int levelOffset) => new Resources(
                gold: 300000 + 100000 * levelOffset,
                wood: 16000 + 4000 * levelOffset,
                stone: 16000 + 4000 * levelOffset,
                ore: 16000 + 4000 * levelOffset
            ));

            return new BuildingDefinition(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Archery range (level {level})"),
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Your archery range increases your archers' strength by 25% for each level; your current bonus is {level * 25}%"),
                costs
            );
        }

        private static BuildingDefinition GenerateCavalryRange() {
            var costs = new ExpressionGenerator<Resources>(new Resources(gold: 20000, wood: 500, stone: 2000, ore: 1000));
            costs.Add(1, new Resources(gold: 30000, wood: 750, stone: 3000, ore: 1500));
            costs.Add(2, new Resources(gold: 50000, wood: 1250, stone: 5000, ore: 2500));
            costs.Add(3, new Resources(gold: 80000, wood: 2000, stone: 8000, ore: 4000));
            costs.Add(4, new Resources(gold: 120000, wood: 6000, stone: 12000, ore: 6000));
            costs.Add(5, new Resources(gold: 200000, wood: 10000, stone: 15000, ore: 10000));
            costs.Add(6, (int level, int levelOffset) => new Resources(
                gold: 300000 + 100000 * levelOffset,
                wood: 16000 + 4000 * levelOffset,
                stone: 16000 + 4000 * levelOffset,
                ore: 16000 + 4000 * levelOffset
            ));

            return new BuildingDefinition(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Cavalry range (level {level})"),
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Your cavalry range increases your cavalry's strength by 25% for each level; your current bonus is {level * 25}%"),
                costs
            );
        }

        private static BuildingDefinition GenerateFootmanRange() {
            var costs = new ExpressionGenerator<Resources>(new Resources(gold: 20000, wood: 500, stone: 2000, ore: 1000));
            costs.Add(1, new Resources(gold: 30000, wood: 750, stone: 3000, ore: 1500));
            costs.Add(2, new Resources(gold: 50000, wood: 1250, stone: 5000, ore: 2500));
            costs.Add(3, new Resources(gold: 80000, wood: 2000, stone: 8000, ore: 4000));
            costs.Add(4, new Resources(gold: 120000, wood: 6000, stone: 12000, ore: 6000));
            costs.Add(5, new Resources(gold: 200000, wood: 10000, stone: 15000, ore: 10000));
            costs.Add(6, (int level, int levelOffset) => new Resources(
                gold: 300000 + 100000 * levelOffset,
                wood: 16000 + 4000 * levelOffset,
                stone: 16000 + 4000 * levelOffset,
                ore: 16000 + 4000 * levelOffset
            ));

            return new BuildingDefinition(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Footmen range (level {level})"),
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Your footmen range increases your footmen's strength by 25% for each level; your current bonus is {level * 25}%"),
                costs
            );
        }

        public static BuildingDefinition Get(BuildingType type) {
            return _buildings[type];
        }
    }
}