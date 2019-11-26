using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Empires;

namespace WarOfEmpires.Domain.Tests.Empires {
    [TestClass]
    public sealed class BuildingTests {
        [TestMethod]
        public void Building_Generates_Name_Correctly() {
            var building = new Building(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Level {level}"),
                new ExpressionGenerator<Resources>((int level, int levelOffset) => new Resources(gold: level * 20000))
            );

            building.GetName(5).Should().Be("Level 5");
        }

        [TestMethod]
        public void Building_Generates_Costs_Correctly() {
            var building = new Building(
                new ExpressionGenerator<string>((int level, int levelOffset) => $"Level {level}"),
                new ExpressionGenerator<Resources>((int level, int levelOffset) => new Resources(gold: level * 20000))
            );

            building.GetNextLevelCost(5).Should().Be(new Resources(gold: 100000));
        }
    }
}