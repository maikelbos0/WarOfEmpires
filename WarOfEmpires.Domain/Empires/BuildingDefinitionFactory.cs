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
            _buildings.Add(BuildingType.Defences, GenerateDefences());
            _buildings.Add(BuildingType.Huts, GenerateHuts());
            _buildings.Add(BuildingType.Barracks, GenerateBarracks());
            _buildings.Add(BuildingType.GoldBank, GenerateGoldBank());
            _buildings.Add(BuildingType.FoodBank, GenerateFoodBank());
            _buildings.Add(BuildingType.WoodBank, GenerateWoodBank());
            _buildings.Add(BuildingType.StoneBank, GenerateStoneBank());
            _buildings.Add(BuildingType.OreBank, GenerateOreBank());
        }

        private static BuildingDefinition GenerateFarm() {
            return new BuildingDefinition(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Farm (level {level})"),
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Your farm increases food production by 25% for each level; your current bonus is {level * 25}%"),
                new ExpressionGenerator<Resources>(SequenceGeneratorFactory.GetGeneratorFunction(new Resources(gold: 10000, wood: 1000, stone: 500, ore: 250))),
                new ExpressionGenerator<int>((int currentLevel, int levelOffset) => currentLevel * 25)
            );
        }

        private static BuildingDefinition GenerateLumberyard() {
            return new BuildingDefinition(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Lumberyard (level {level})"),
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Your lumberyard increases wood production by 25% for each level; your current bonus is {level * 25}%"),
                new ExpressionGenerator<Resources>(SequenceGeneratorFactory.GetGeneratorFunction(new Resources(gold: 10000, wood: 1000, stone: 500, ore: 250))),
                new ExpressionGenerator<int>((int currentLevel, int levelOffset) => currentLevel * 25)
            );
        }

        private static BuildingDefinition GenerateQuarry() {
            return new BuildingDefinition(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Quarry (level {level})"),
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Your quarry increases stone production by 25% for each level; your current bonus is {level * 25}%"),
                new ExpressionGenerator<Resources>(SequenceGeneratorFactory.GetGeneratorFunction(new Resources(gold: 10000, wood: 1000, stone: 500, ore: 250))),
                new ExpressionGenerator<int>((int currentLevel, int levelOffset) => currentLevel * 25)
            );
        }

        private static BuildingDefinition GenerateMine() {
            return new BuildingDefinition(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Mine (level {level})"),
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Your mine increases ore production by 25% for each level; your current bonus is {level * 25}%"),
                new ExpressionGenerator<Resources>(SequenceGeneratorFactory.GetGeneratorFunction(new Resources(gold: 10000, wood: 1000, stone: 500, ore: 250))),
                new ExpressionGenerator<int>((int currentLevel, int levelOffset) => currentLevel * 25)
            );
        }

        private static BuildingDefinition GenerateForge() {
            return new BuildingDefinition(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Forge (level {level})"),
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Your forge increases your troops' attack strength by 25% for each level; your current bonus is {level * 25}%"),
                new ExpressionGenerator<Resources>(SequenceGeneratorFactory.GetGeneratorFunction(new Resources(gold: 10000, wood: 250, stone: 500, ore: 1000))),
                new ExpressionGenerator<int>((int currentLevel, int levelOffset) => currentLevel * 10)
            );
        }

        private static BuildingDefinition GenerateArmoury() {
            return new BuildingDefinition(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Armoury (level {level})"),
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Your armoury increases your troops' defensive strength by 25% for each level; your current bonus is {level * 25}%"),
                new ExpressionGenerator<Resources>(SequenceGeneratorFactory.GetGeneratorFunction(new Resources(gold: 10000, wood: 250, stone: 500, ore: 1000))),
                new ExpressionGenerator<int>((int currentLevel, int levelOffset) => currentLevel * 10)
            );
        }

        private static BuildingDefinition GenerateArcheryRange() {
            return new BuildingDefinition(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Archery range (level {level})"),
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Your archery range increases your archers' strength by 25% for each level; your current bonus is {level * 25}%"),
                new ExpressionGenerator<Resources>(SequenceGeneratorFactory.GetGeneratorFunction(new Resources(gold: 10000, wood: 250, stone: 500, ore: 1000))),
                new ExpressionGenerator<int>((int currentLevel, int levelOffset) => currentLevel * 10)
            );
        }

        private static BuildingDefinition GenerateCavalryRange() {
            return new BuildingDefinition(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Cavalry range (level {level})"),
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Your cavalry range increases your cavalry's strength by 25% for each level; your current bonus is {level * 25}%"),
                new ExpressionGenerator<Resources>(SequenceGeneratorFactory.GetGeneratorFunction(new Resources(gold: 10000, wood: 250, stone: 500, ore: 1000))),
                new ExpressionGenerator<int>((int currentLevel, int levelOffset) => currentLevel * 10)
            );
        }

        private static BuildingDefinition GenerateFootmanRange() {
            return new BuildingDefinition(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Footmen range (level {level})"),
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Your footmen range increases your footmen's strength by 25% for each level; your current bonus is {level * 25}%"),
                new ExpressionGenerator<Resources>(SequenceGeneratorFactory.GetGeneratorFunction(new Resources(gold: 10000, wood: 250, stone: 500, ore: 1000))),
                new ExpressionGenerator<int>((int currentLevel, int levelOffset) => currentLevel * 10)
            );
        }

        private static BuildingDefinition GenerateDefences() {
            var names = new ExpressionGenerator<string>("Tent");
            names.Add(1, "Armed Camp");
            names.Add(2, "Stockade");
            names.Add(3, "Fort");
            names.Add(4, "Walled Fort");
            names.Add(5, "Walled Fort with Moat");
            names.Add(6, "Keep");
            names.Add(7, "Keep with Wall");
            names.Add(8, "Keep with Wall and Moat");
            names.Add(9, "Castle");
            names.Add(10, "Castle with Wall");
            names.Add(11, "Castle with Wall and Moat");
            names.Add(12, "Fortress");
            names.Add(13, "Fortress with Wall");
            names.Add(14, "Fortress with Wall and Moat");
            names.Add(15, (int level, int levelOffset) => $"Citadel (level {levelOffset + 1})");

            return new BuildingDefinition(
                names,
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Your defences protect against castle attacks and increase your recruiting by 1 peasant for each level; your current bonus is {level}"),
                new ExpressionGenerator<Resources>(SequenceGeneratorFactory.GetGeneratorFunction(new Resources(gold: 2000, wood: 1000, stone: 2000, ore: 500))),
                new ExpressionGenerator<int>((int currentLevel, int levelOffset) => currentLevel * 1)
            );
        }

        private static BuildingDefinition GenerateHuts() {
            var costs = new ExpressionGenerator<Resources>((int level, int levelOffset) => new Resources(gold: 2500 + 2500 * level, wood: 250 + 250 * level, stone: 100 + 100 * level, ore: 50 + 50 * level));

            return new BuildingDefinition(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Peasant huts (level {level})"),
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Your peasant huts provide the housing for your workers and peasants; each hut houses 10 peasants and your current capacity is {level * 10}"),
                costs,
                new ExpressionGenerator<int>((int currentLevel, int levelOffset) => currentLevel * 10)
            );
        }

        private static BuildingDefinition GenerateBarracks() {
            var costs = new ExpressionGenerator<Resources>((int level, int levelOffset) => new Resources(gold: 2500 + 2500 * level, wood: 100 + 100 * level, stone: 250 + 250 * level, ore: 50 + 50 * level));

            return new BuildingDefinition(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Baracks (level {level})"),
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Your barracks provide the housing for your troops; each barracks houses 10 troops and your current capacity is {level * 10}"),
                costs,
                new ExpressionGenerator<int>((int currentLevel, int levelOffset) => currentLevel * 10)
            );
        }
        
        private static BuildingDefinition GenerateGoldBank() {
            var descriptions = new ExpressionGenerator<string>("Your gold bank allows you to safely store gold away from attackers");
            descriptions.Add(1, SequenceGeneratorFactory.GetGeneratorFunction((value) => $"Your gold bank allows you to safely store gold away from attackers; your current capacity is {value * 25000} gold"));

            var bonuses = new ExpressionGenerator<int>(0);
            bonuses.Add(1, SequenceGeneratorFactory.GetGeneratorFunction(25000));

            return new BuildingDefinition(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Gold bank (level {level})"),
                descriptions,
                new ExpressionGenerator<Resources>(SequenceGeneratorFactory.GetGeneratorFunction(new Resources(gold: 10000, wood: 250, stone: 1000, ore: 500))),
                bonuses
            );
        }

        private static BuildingDefinition GenerateFoodBank() {
            var descriptions = new ExpressionGenerator<string>("Your food bank allows you to safely store food away from attackers");
            descriptions.Add(1, SequenceGeneratorFactory.GetGeneratorFunction((value) => $"Your food bank allows you to safely store food away from attackers; your current capacity is {value * 10000} food"));

            var bonuses = new ExpressionGenerator<int>(0);
            bonuses.Add(1, SequenceGeneratorFactory.GetGeneratorFunction(10000));

            return new BuildingDefinition(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Food bank (level {level})"),
                descriptions,
                new ExpressionGenerator<Resources>(SequenceGeneratorFactory.GetGeneratorFunction(new Resources(gold: 10000, wood: 500, stone: 1000, ore: 250))),
                bonuses
            );
        }

        private static BuildingDefinition GenerateWoodBank() {
            var descriptions = new ExpressionGenerator<string>("Your wood bank allows you to safely store wood away from attackers");
            descriptions.Add(1, SequenceGeneratorFactory.GetGeneratorFunction((value) => $"Your wood bank allows you to safely store wood away from attackers; your current capacity is {value * 10000} wood"));

            var bonuses = new ExpressionGenerator<int>(0);
            bonuses.Add(1, SequenceGeneratorFactory.GetGeneratorFunction(10000));

            return new BuildingDefinition(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Wood bank (level {level})"),
                descriptions,
                new ExpressionGenerator<Resources>(SequenceGeneratorFactory.GetGeneratorFunction(new Resources(gold: 10000, wood: 500, stone: 1000, ore: 250))),
                bonuses
            );
        }

        private static BuildingDefinition GenerateStoneBank() {
            var descriptions = new ExpressionGenerator<string>("Your stone bank allows you to safely store stone away from attackers");
            descriptions.Add(1, SequenceGeneratorFactory.GetGeneratorFunction((value) => $"Your stone bank allows you to safely store stone away from attackers; your current capacity is {value * 10000} stone"));

            var bonuses = new ExpressionGenerator<int>(0);
            bonuses.Add(1, SequenceGeneratorFactory.GetGeneratorFunction(10000));

            return new BuildingDefinition(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Stone bank (level {level})"),
                descriptions,
                new ExpressionGenerator<Resources>(SequenceGeneratorFactory.GetGeneratorFunction(new Resources(gold: 10000, wood: 500, stone: 1000, ore: 250))),
                bonuses
            );
        }

        private static BuildingDefinition GenerateOreBank() {
            var descriptions = new ExpressionGenerator<string>("Your ore bank allows you to safely store ore away from attackers");
            descriptions.Add(1, SequenceGeneratorFactory.GetGeneratorFunction((value) => $"Your ore bank allows you to safely store ore away from attackers; your current capacity is {value * 10000} ore"));

            var bonuses = new ExpressionGenerator<int>(0);
            bonuses.Add(1, SequenceGeneratorFactory.GetGeneratorFunction(10000));

            return new BuildingDefinition(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Ore bank (level {level})"),
                descriptions,
                new ExpressionGenerator<Resources>(SequenceGeneratorFactory.GetGeneratorFunction(new Resources(gold: 10000, wood: 500, stone: 1000, ore: 250))),
                bonuses
            );
        }

        public static BuildingDefinition Get(BuildingType type) {
            return _buildings[type];
        }
    }
}