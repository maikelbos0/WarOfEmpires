using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Markets;

namespace WarOfEmpires.Domain.Tests.Markets {
    [TestClass]
    public sealed class MerchandiseTotalsTests {
        [TestMethod]
        public void MerchandiseTotals_Constructor_Succeeds() {
            var totals = new MerchandiseTotals(MerchandiseType.Wood, 50000, 15);

            totals.Type.Should().Be(MerchandiseType.Wood);
            totals.Quantity.Should().Be(50000);
            totals.Price.Should().Be(15);
        }

        [DataTestMethod]
        [DataRow(-1, 1, DisplayName = "Negative quantity")]
        [DataRow(0, 1, DisplayName = "Zero quantity")]
        [DataRow(1, -1, DisplayName = "Negative price")]
        [DataRow(1, 0, DisplayName = "Zero price")]
        public void MerchandiseTotals_Constructor_Throws_Exception_For_Negative_Values(int quantity, int price) {
            Action action = () => _ = new MerchandiseTotals(MerchandiseType.Wood, quantity, price);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [DataTestMethod]
        [DataRow(MerchandiseType.Food, 1, 0, 1, 0, 0, 0, DisplayName = "Food")]
        [DataRow(MerchandiseType.Wood, 1, 0, 0, 1, 0, 0, DisplayName = "Wood")]
        [DataRow(MerchandiseType.Stone, 1, 0, 0, 0, 1, 0, DisplayName = "Stone")]
        [DataRow(MerchandiseType.Ore, 1, 0, 0, 0, 0, 1, DisplayName = "Ore")]
        public void MerchandiseTotals_ToResources_Succeeds(MerchandiseType type, int quantity, int expectedGold, int expectedFood, int expectedWood, int expectedStone, int expectedOre) {
            var totals = new MerchandiseTotals(type, quantity, 5);

            totals.ToResources().Should().Be(new Resources(expectedGold, expectedFood, expectedWood, expectedStone, expectedOre));
        }
    }
}