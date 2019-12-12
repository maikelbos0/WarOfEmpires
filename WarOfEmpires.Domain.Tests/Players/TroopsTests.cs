using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Players {
    [TestClass]
    public sealed class TroopsTests {
        [DataTestMethod]
        [DataRow(0, 0, DisplayName = "Empty")]
        [DataRow(10, 10, DisplayName = "Not empty")]
        public void Troops_Constructor_Succeeds(int soldiers, int mercenaries) {
            var troops = new Troops(soldiers, mercenaries);

            troops.Soldiers.Should().Be(soldiers);
            troops.Mercenaries.Should().Be(mercenaries);
        }

        [DataTestMethod]
        [DataRow(-1, 0, DisplayName = "Soldiers")]
        [DataRow(0, -1, DisplayName = "Mercenaries")]
        public void Troops_Constructor_Throws_Exception_For_Negative_Values(int soldiers, int mercenaries) {
            Action action = () => new Troops(soldiers, mercenaries);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Troops_Add_Operator_Works() {
            var troops1 = new Troops(1, 2);
            var troops2 = new Troops(10, 20);

            var troops = troops1 + troops2;

            troops.Soldiers.Should().Be(11);
            troops.Mercenaries.Should().Be(22);
        }

        [TestMethod]
        public void Troops_Subtract_Operator_Works() {
            var troops1 = new Troops(10, 20);
            var troops2 = new Troops(1, 2);

            var troops = troops1 - troops2;

            troops.Soldiers.Should().Be(9);
            troops.Mercenaries.Should().Be(18);
        }

        [DataTestMethod]
        [DataRow(0, 0, 0, 0, 0, DisplayName = "Empty 1")]
        [DataRow(0, 0, 5, 0, 0, DisplayName = "Empty 2")]
        [DataRow(15, 5, 3, 0, 3, DisplayName = "Some mercenaries")]
        [DataRow(15, 5, 5, 0, 5, DisplayName = "All mercenaries")]
        [DataRow(15, 5, 6, 1, 5, DisplayName = "Some soldiers")]
        [DataRow(15, 5, 20, 15, 5, DisplayName = "All soldiers")]
        [DataRow(15, 5, 25, 15, 5, DisplayName = "Too many")]
        public void Troops_GetTroopCasualties_Succeeds(int soldiers, int mercenaries, int casualties, int expectedSoldierCasualties, int expectedMercenaryCasualties) {
            var troops = new Troops(soldiers, mercenaries);
            var result = troops.GetTroopCasualties(casualties);

            result.Soldiers.Should().Be(expectedSoldierCasualties);
            result.Mercenaries.Should().Be(expectedMercenaryCasualties);
        }

        [TestMethod]
        public void Troops_GetTotals_Succeeds() {
            var troops = new Troops(5, 2);

            troops.GetTotals().Should().Be(7);
        }
    }
}