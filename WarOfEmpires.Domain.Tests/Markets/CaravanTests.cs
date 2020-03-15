using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Markets {
    [TestClass]
    public sealed class CaravanTests {
        [TestMethod]
        public void Caravan_GetRemainingCapacity_Succeeds_When_Empty() {
            var player = new Player(0, "Test");
            var caravan = new Caravan(player);

            caravan.GetRemainingCapacity(25000).Should().Be(25000);
        }

        [TestMethod]
        public void Caravan_GetRemainingCapacity_Succeeds() {
            var player = new Player(0, "Test");
            var caravan = new Caravan(player);

            caravan.Merchandise.Add(new Merchandise(MerchandiseType.Food, 8000, 5));
            caravan.Merchandise.Add(new Merchandise(MerchandiseType.Wood, 7000, 5));

            caravan.GetRemainingCapacity(25000).Should().Be(10000);
        }

        [TestMethod]
        public void Caravan_GetRemainingCapacity_Succeeds_When_Full() {
            var player = new Player(0, "Test");
            var caravan = new Caravan(player);

            caravan.Merchandise.Add(new Merchandise(MerchandiseType.Food, 10000, 5));
            caravan.Merchandise.Add(new Merchandise(MerchandiseType.Wood, 6000, 5));
            caravan.Merchandise.Add(new Merchandise(MerchandiseType.Stone, 5000, 5));
            caravan.Merchandise.Add(new Merchandise(MerchandiseType.Ore, 4000, 5));

            caravan.GetRemainingCapacity(25000).Should().Be(0);
        }

        // TODO write tests for withdraw

        [TestMethod]
        public void Caravan_Buy_Succeeds() {
            var seller = new Player(1, "Seller");
            var buyer = new Player(2, "Buyer");
            var caravan = new Caravan(seller);

            seller.Caravans.Add(caravan);
            caravan.Merchandise.Add(new Merchandise(MerchandiseType.Wood, 1000, 6));

            caravan.Buy(buyer, MerchandiseType.Wood, 600);

            caravan.Merchandise.Should().HaveCount(1);
            caravan.Merchandise.Single().Quantity.Should().Be(400);
        }

        [TestMethod]
        public void Caravan_Buy_Removes_Merchandise_If_Empty() {
            var seller = new Player(1, "Seller");
            var buyer = new Player(2, "Buyer");
            var caravan = new Caravan(seller);

            seller.Caravans.Add(caravan);
            caravan.Merchandise.Add(new Merchandise(MerchandiseType.Food, 1000, 5));
            caravan.Merchandise.Add(new Merchandise(MerchandiseType.Wood, 1000, 6));

            caravan.Buy(buyer, MerchandiseType.Wood, 1000);

            caravan.Merchandise.Should().HaveCount(1);
            caravan.Merchandise.Single().Type.Should().Be(MerchandiseType.Food);
        }

        [TestMethod]
        public void Caravan_Buy_Removes_Self_From_Player_If_Empty() {
            var seller = new Player(1, "Seller");
            var buyer = new Player(2, "Buyer");
            var caravan = new Caravan(seller);

            seller.Caravans.Add(caravan);
            caravan.Merchandise.Add(new Merchandise(MerchandiseType.Wood, 1000, 6));

            caravan.Buy(buyer, MerchandiseType.Wood, 1000);

            seller.Caravans.Should().HaveCount(0);
        }
    }
}