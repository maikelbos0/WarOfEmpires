using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Players {
    [TestClass]
    public sealed class TroopStrengthTests {
        [DataTestMethod]
        [DataRow(0, 0, DisplayName = "Empty")]
        [DataRow(100, 100, DisplayName = "Not empty")]
        public void TroopStrength_Constructor_Succeeds(int attack, int defense) {
            var troopStrength = new TroopStrength(attack, defense);

            troopStrength.Attack.Should().Be(attack);
            troopStrength.Defense.Should().Be(defense);
        }

        [DataTestMethod]
        [DataRow(-1, 0, DisplayName = "Attack")]
        [DataRow(0, -1, DisplayName = "Defense")]
        public void TroopStrength_Constructor_Throws_Exception_For_Negative_Values(int attack, int defense) {
            Action action = () => new TroopStrength(attack, defense);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }
        [TestMethod]

        public void TroopStrength_Multiply_Operator_Works() {
            // Use int * TroopStrength operator to test both overloads
            var troopStrength = 15 * new TroopStrength(100, 200);

            troopStrength.Attack.Should().Be(1500);
            troopStrength.Defense.Should().Be(3000);
        }
    }
}