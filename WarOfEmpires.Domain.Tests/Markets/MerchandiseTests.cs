using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
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

        [TestMethod]
        public void Merchandise_Buy_Adds_History() {
            var seller = new Player(1, "Seller");
            var buyer = new Player(2, "Buyer");
            var merchandise = new Merchandise(MerchandiseType.Wood, 2000, 5);

            merchandise.Buy(seller, buyer, 800);

            seller.SellTransactions.Should().HaveCount(1);
            buyer.BuyTransactions.Should().HaveCount(1);

            var transaction = seller.SellTransactions.First();

            buyer.BuyTransactions.First().Should().Be(transaction);

            transaction.Type.Should().Be(MerchandiseType.Wood);
            transaction.Price.Should().Be(5);
            transaction.Quantity.Should().Be(800);
            transaction.IsRead.Should().BeFalse();
        }

        [TestMethod]
        public void Merchandise_Buy_Sets_Seller_HasNewMarketSales_True() {
            var seller = new Player(1, "Seller");
            var buyer = new Player(2, "Buyer");
            var merchandise = new Merchandise(MerchandiseType.Wood, 2000, 5);

            merchandise.Buy(seller, buyer, 800);

            seller.HasNewMarketSales.Should().BeTrue();
            buyer.HasNewMarketSales.Should().BeFalse();
        }
    }
}