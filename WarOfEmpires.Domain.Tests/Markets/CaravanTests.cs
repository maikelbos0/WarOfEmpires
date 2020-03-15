using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        // TODO write tests for merchandise sell (+ player integration?)
    }
}