﻿using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Domain.Attacks;

namespace WarOfEmpires.Domain.Tests.Attacks {
    [TestClass]
    public sealed class TroopsTests {
        [DataTestMethod]
        [DataRow(0, 0, 0, 0, 0, DisplayName = "Empty 1")]
        [DataRow(0, 0, 5, 0, 0, DisplayName = "Empty 2")]
        [DataRow(15, 5, 3, 0, 3, DisplayName = "Some mercenaries")]
        [DataRow(15, 5, 5, 0, 5, DisplayName = "All mercenaries")]
        [DataRow(15, 5, 6, 1, 5, DisplayName = "Some soldiers")]
        [DataRow(15, 5, 20, 15, 5, DisplayName = "All soldiers")]
        [DataRow(15, 5, 25, 15, 5, DisplayName = "Too many")]
        public void Troops_ProcessCasualties_Succeeds(int soldiers, int mercenaries, int casualties, int expectedSoldierCasualties, int expectedMercenaryCasualties) {
            var troops = new Troops(TroopType.Archers, soldiers, mercenaries);
            var result = troops.ProcessCasualties(casualties);

            result.Soldiers.Should().Be(expectedSoldierCasualties);
            result.Mercenaries.Should().Be(expectedMercenaryCasualties);

            troops.Soldiers.Should().Be(soldiers - expectedSoldierCasualties);
            troops.Mercenaries.Should().Be(mercenaries - expectedMercenaryCasualties);
        }

        [TestMethod]
        public void Troops_Train_Succeeds() {
            var troops = new Troops(TroopType.Archers, 15, 5);

            troops.Train(3, 1);

            troops.Soldiers.Should().Be(18);
            troops.Mercenaries.Should().Be(6);
        }

        [TestMethod]
        public void Troops_Untrain_Succeeds() {
            var troops = new Troops(TroopType.Archers, 15, 5);

            troops.Untrain(3, 1);

            troops.Soldiers.Should().Be(12);
            troops.Mercenaries.Should().Be(4);
        }

        [TestMethod]
        public void Troops_GetTotals_Succeeds() {
            var troops = new Troops(TroopType.Archers, 5, 2);

            troops.GetTotals().Should().Be(7);
        }
    }
}