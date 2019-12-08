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
            var resources = new TroopStrength(attack, defense);

            resources.Attack.Should().Be(attack);
            resources.Defense.Should().Be(defense);
        }

        [DataTestMethod]
        [DataRow(-1, 0, DisplayName = "Attack")]
        [DataRow(0, -1, DisplayName = "Defense")]
        public void TroopStrength_Constructor_Throws_Exception_For_Negative_Values(int attack, int defense) {
            Action action = () => new TroopStrength(attack, defense);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}