using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Markets {
    [TestClass]
    public sealed class MerchandiseTests {
        [TestMethod]
        public void Merchandise_Buy_Succeeds() {
            var seller = new Player(1, "Seller");
            var buyer = new Player(2, "Buyer");
            var merchandise = new Merchandise(MerchandiseType.Wood, 2000, 5);
            var previousSellerResources = seller.Resources;
            var previousBuyerResources = buyer.Resources;

            merchandise.Buy(seller, buyer, 800);

            merchandise.Quantity.Should().Be(1200);
            seller.Resources.Should().Be(previousSellerResources + new Resources(gold: 3400));
            buyer.Resources.Should().Be(previousBuyerResources + new Resources(wood: 800) - new Resources(gold: 4000));
        }
    }
}