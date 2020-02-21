using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Empires;

namespace WarOfEmpires.Domain.Tests.Empires {
    [TestClass]
    public sealed class BuildingDefinitionTests {
        [TestMethod]
        public void BuildingDefinition_Generates_Name_Correctly() {
            var building = new BuildingDefinition(
                BuildingType.Forge,
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Level {level}"),
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Description {level}"),
                new ExpressionGenerator<Resources>((int level, int levelOffset) => new Resources(gold: level * 20000)),
                new ExpressionGenerator<int>((int currentLevel, int levelOffset) => currentLevel * 10)
            );

            building.GetName(5).Should().Be("Level 5");
        }

        [TestMethod]
        public void BuildingDefinition_Generates_Description_Correctly() {
            var building = new BuildingDefinition(
                BuildingType.Forge,
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Level {level}"),
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Description {level}"),
                new ExpressionGenerator<Resources>((int level, int levelOffset) => new Resources(gold: level * 20000)),
                new ExpressionGenerator<int>((int currentLevel, int levelOffset) => currentLevel * 10)
            );

            building.GetDescription(5).Should().Be("Description 5");
        }

        [TestMethod]
        public void BuildingDefinition_Generates_Costs_Correctly() {
            var building = new BuildingDefinition(
                BuildingType.Forge,
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Level {level}"),
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Description {level}"),
                new ExpressionGenerator<Resources>((int level, int levelOffset) => new Resources(gold: level * 20000)),
                new ExpressionGenerator<int>((int currentLevel, int levelOffset) => currentLevel * 10)
            );

            building.GetNextLevelCost(5).Should().Be(new Resources(gold: 100000));
        }

        [TestMethod]
        public void BuildingDefinition_Generates_Bonus_Correctly() {
            var building = new BuildingDefinition(
                BuildingType.Forge,
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Level {level}"),
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Description {level}"),
                new ExpressionGenerator<Resources>((int level, int levelOffset) => new Resources(gold: level * 20000)),
                new ExpressionGenerator<int>((int currentLevel, int levelOffset) => currentLevel * 23)
            );

            building.GetBonus(5).Should().Be(115);
        }
    }
}