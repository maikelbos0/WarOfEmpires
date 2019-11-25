using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Domain.Common;

namespace WarOfEmpires.Domain.Tests.Common {
    [TestClass]
    public sealed class ResourcesTests {
        [DataTestMethod]
        [DataRow(0, 0, 0, 0, 0, DisplayName = "Empty")]
        [DataRow(1000, 1000, 1000, 1000, 1000, DisplayName = "Not empty")]
        public void Resources_Constructor_Succeeds(int gold, int food, int wood, int stone, int ore) {
            var resources = new Resources(gold, food, wood, stone, ore);

            resources.Gold.Should().Be(gold);
            resources.Food.Should().Be(food);
            resources.Wood.Should().Be(wood);
            resources.Stone.Should().Be(stone);
            resources.Ore.Should().Be(ore);
        }

        [DataTestMethod]
        [DataRow(-1, 0, 0, 0, 0, DisplayName = "Gold")]
        [DataRow(0, -1, 0, 0, 0, DisplayName = "Food")]
        [DataRow(0, 0, -1, 0, 0, DisplayName = "Wood")]
        [DataRow(0, 0, 0, -1, 0, DisplayName = "Stone")]
        [DataRow(0, 0, 0, 0, -1, DisplayName = "Ore")]
        public void Resources_Constructor_Throws_Exception_For_Negative_Values(int gold, int food, int wood, int stone, int ore) {
            Action action = () => new Resources(gold, food, wood, stone, ore);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Resources_Add_Operator_Works() {
            var resources1 = new Resources(1, 2, 3, 4, 5);
            var resources2 = new Resources(10, 20, 30, 40, 50);

            var resources = resources1 + resources2;

            resources.Gold.Should().Be(11);
            resources.Food.Should().Be(22);
            resources.Wood.Should().Be(33);
            resources.Stone.Should().Be(44);
            resources.Ore.Should().Be(55);
        }

        [TestMethod]
        public void Resources_Subtract_Operator_Works() {
            var resources1 = new Resources(10, 20, 30, 40, 50);
            var resources2 = new Resources(1, 2, 3, 4, 5);

            var resources = resources1 - resources2;

            resources.Gold.Should().Be(9);
            resources.Food.Should().Be(18);
            resources.Wood.Should().Be(27);
            resources.Stone.Should().Be(36);
            resources.Ore.Should().Be(45);
        }

        [TestMethod]
        public void Resources_Multiply_Operator_Works() {
            // Use int * Resources operator to test both overloads
            var resources = 15 * new Resources(10, 20, 30, 40, 50);
                       
            resources.Gold.Should().Be(150);
            resources.Food.Should().Be(300);
            resources.Wood.Should().Be(450);
            resources.Stone.Should().Be(600);
            resources.Ore.Should().Be(750);
        }

        [DataTestMethod]
        [DataRow(1000, 0, 0, 0, 0, true, DisplayName = "Gold affordable")]
        [DataRow(0, 1000, 0, 0, 0, true, DisplayName = "Food affordable")]
        [DataRow(0, 0, 1000, 0, 0, true, DisplayName = "Wood affordable")]
        [DataRow(0, 0, 0, 1000, 0, true, DisplayName = "Stone affordable")]
        [DataRow(0, 0, 0, 0, 1000, true, DisplayName = "Ore affordable")]
        [DataRow(100000, 0, 0, 0, 0, false, DisplayName = "Gold too much")]
        [DataRow(0, 100000, 0, 0, 0, false, DisplayName = "Food too much")]
        [DataRow(0, 0, 100000, 0, 0, false, DisplayName = "Wood too much")]
        [DataRow(0, 0, 0, 100000, 0, false, DisplayName = "Stone too much")]
        [DataRow(0, 0, 0, 0, 100000, false, DisplayName = "Ore too much")]
        public void Resources_CanAfford_Works(int gold, int food, int wood, int stone, int ore, bool result) {
            var resources = new Resources(1000, 1000, 1000, 1000, 1000);

            resources.CanAfford(new Resources(gold, food, wood, stone, ore)).Should().Be(result);
        }
    }
}