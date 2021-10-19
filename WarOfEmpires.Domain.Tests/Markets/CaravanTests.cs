using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Markets {
    [TestClass]
    public sealed class CaravanTests {
        [TestMethod]
        public void Caravan_GetRemainingCapacity_Succeeds_When_Empty() {
            var player = new Player(0, "Test", Race.Elves);
            var caravan = new Caravan(player);

            caravan.GetRemainingCapacity(25000).Should().Be(25000);
        }

        [TestMethod]
        public void Caravan_GetRemainingCapacity_Succeeds() {
            var player = new Player(0, "Test", Race.Elves);
            var caravan = new Caravan(player);

            caravan.Merchandise.Add(new Merchandise(MerchandiseType.Food, 8000, 5));
            caravan.Merchandise.Add(new Merchandise(MerchandiseType.Wood, 7000, 5));

            caravan.GetRemainingCapacity(25000).Should().Be(10000);
        }

        [TestMethod]
        public void Caravan_GetRemainingCapacity_Succeeds_When_Full() {
            var player = new Player(0, "Test", Race.Elves);
            var caravan = new Caravan(player);

            caravan.Merchandise.Add(new Merchandise(MerchandiseType.Food, 10000, 5));
            caravan.Merchandise.Add(new Merchandise(MerchandiseType.Wood, 6000, 5));
            caravan.Merchandise.Add(new Merchandise(MerchandiseType.Stone, 5000, 5));
            caravan.Merchandise.Add(new Merchandise(MerchandiseType.Ore, 4000, 5));

            caravan.GetRemainingCapacity(25000).Should().Be(0);
        }

        [TestMethod]
        public void Caravan_Withdraw_Succeeds() {
            var player = new Player(0, "Test", Race.Elves);
            var previousResources = player.Resources;
            var caravan = new Caravan(player);

            player.Caravans.Add(caravan);

            caravan.Merchandise.Add(new Merchandise(MerchandiseType.Food, 10000, 5));
            caravan.Merchandise.Add(new Merchandise(MerchandiseType.Wood, 6000, 5));
            caravan.Merchandise.Add(new Merchandise(MerchandiseType.Stone, 5000, 5));
            caravan.Merchandise.Add(new Merchandise(MerchandiseType.Ore, 4000, 5));

            caravan.Withdraw();

            foreach (var merchandise in caravan.Merchandise) {
                merchandise.Quantity.Should().Be(0);
            }

            player.Resources.Should().Be(previousResources + new Resources(food: 10000, wood: 6000, stone: 5000, ore: 4000));
            player.Caravans.Should().BeEmpty();
        }

        [DataTestMethod]
        [DataRow(0, 10000, 10000, DisplayName = "No time")]
        [DataRow(1, 8333, 9375, DisplayName = "1 hour")]
        [DataRow(5, 5000, 7500, DisplayName = "5 hours")]
        [DataRow(15, 2500, 5000, DisplayName = "15 hours")]
        [DataRow(24, 1724, 3846, DisplayName = "24 hours")]
        public void Caravan_Withdraw_Destroys_Randomly_Based_On_Duration(int hours, int expectedMinimum, int expectedMaximum) {
            var player = new Player(0, "Test", Race.Elves);
            var previousFood = player.Resources.Food;
            var caravan = new Caravan(player);

            caravan.Merchandise.Add(new Merchandise(MerchandiseType.Food, 10000, 5));
            typeof(Caravan).GetProperty(nameof(Caravan.Date)).SetValue(caravan, DateTime.UtcNow.AddHours(-hours));

            caravan.Withdraw();

            player.Resources.Food.Should().BeInRange(previousFood + expectedMinimum, previousFood + expectedMaximum);
        }

        [TestMethod]
        public void Caravan_Buy_Succeeds_Partial_Purchase() {
            var seller = new Player(1, "Seller", Race.Elves);
            var buyer = new Player(2, "Buyer", Race.Elves);
            var caravan = new Caravan(seller);

            seller.Caravans.Add(caravan);
            caravan.Merchandise.Add(new Merchandise(MerchandiseType.Wood, 1000, 6));

            var remainder = caravan.Buy(buyer, MerchandiseType.Wood, 600);

            caravan.Merchandise.Should().HaveCount(1);
            caravan.Merchandise.Single().Quantity.Should().Be(400);
            remainder.Should().Be(0);
        }

        [TestMethod]
        public void Caravan_Buy_Succeeds_Full_Purchase() {
            var seller = new Player(1, "Seller", Race.Elves);
            var buyer = new Player(2, "Buyer", Race.Elves);
            var caravan = new Caravan(seller);

            seller.Caravans.Add(caravan);
            caravan.Merchandise.Add(new Merchandise(MerchandiseType.Wood, 1000, 6));

            var remainder = caravan.Buy(buyer, MerchandiseType.Wood, 1600);

            caravan.Merchandise.Should().BeEmpty();
            remainder.Should().Be(600);
        }
    }
}