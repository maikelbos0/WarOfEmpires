using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Markets;

namespace WarOfEmpires.Domain.Tests.Markets {
    [TestClass]
    public sealed class BlackMarketMerchandiseTotalsTests {
        [TestMethod]
        public void BlackMarketMerchandiseTotals_Constructor_Succeeds() {
            var totals = new BlackMarketMerchandiseTotals(MerchandiseType.Wood, 50000);

            totals.Type.Should().Be(MerchandiseType.Wood);
            totals.Quantity.Should().Be(50000);
        }

        [DataTestMethod]
        [DataRow(-1, DisplayName = "Negative quantity")]
        [DataRow(0, DisplayName = "Zero quantity")]
        public void BlackMarketMerchandiseTotals_Constructor_Throws_Exception_For_Negative_Values(int quantity) {
            Action action = () => new BlackMarketMerchandiseTotals(MerchandiseType.Wood, quantity);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [DataTestMethod]
        [DataRow(MerchandiseType.Food, 1, 0, 1, 0, 0, 0, DisplayName = "Food")]
        [DataRow(MerchandiseType.Wood, 1, 0, 0, 1, 0, 0, DisplayName = "Wood")]
        [DataRow(MerchandiseType.Stone, 1, 0, 0, 0, 1, 0, DisplayName = "Stone")]
        [DataRow(MerchandiseType.Ore, 1, 0, 0, 0, 0, 1, DisplayName = "Ore")]
        public void BlackMarketMerchandiseTotals_ToResources_Succeeds(MerchandiseType type, int quantity, int expectedGold, int expectedFood, int expectedWood, int expectedStone, int expectedOre) {
            var totals = new BlackMarketMerchandiseTotals(type, quantity);

            totals.ToResources().Should().Be(new Resources(expectedGold, expectedFood, expectedWood, expectedStone, expectedOre));
        }
    }
}