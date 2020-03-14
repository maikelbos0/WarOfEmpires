using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
        [DataRow(-1, 0, DisplayName = "Quantity")]
        [DataRow(0, -1, DisplayName = "Price")]
        public void MerchandiseTotals_Constructor_Throws_Exception_For_Negative_Values(int quantity, int price) {
            Action action = () => new MerchandiseTotals(MerchandiseType.Wood, quantity, price);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void MerchandiseTotals_Subtract_Operator_Works() {
            var totals = new MerchandiseTotals(MerchandiseType.Wood, 15000, 15);

            var newTotals = totals - 5000;

            newTotals.Type.Should().Be(MerchandiseType.Wood);
            newTotals.Quantity.Should().Be(10000);
            newTotals.Price.Should().Be(15);
        }
    }
}