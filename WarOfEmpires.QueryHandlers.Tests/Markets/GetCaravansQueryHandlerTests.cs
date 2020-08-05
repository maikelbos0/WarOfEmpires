using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Queries.Markets;
using WarOfEmpires.QueryHandlers.Markets;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Markets {
    [TestClass]
    public sealed class GetCaravansQueryHandlerTests {
        [TestMethod]
        public void GetCaravansQueryHandler_Returns_Existing_Caravans() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithCaravan(1, new Merchandise(MerchandiseType.Food, 1000, 10), new Merchandise(MerchandiseType.Wood, 2000, 9), new Merchandise(MerchandiseType.Stone, 3000, 8), new Merchandise(MerchandiseType.Ore, 4000, 7))
                .WithCaravan(2, new Merchandise(MerchandiseType.Ore, 25000, 4));

            var handler = new GetCaravansQueryHandler(builder.Context);
            var query = new GetCaravansQuery("test1@test.com");

            var result = handler.Execute(query).ToList();

            result.Should().HaveCount(2);

            result[0].Id.Should().Be(1);
            result[0].Date.Should().BeCloseTo(DateTime.UtcNow, 1000);
            result[0].Food.Should().Be(1000);
            result[0].FoodPrice.Should().Be(10);
            result[0].Wood.Should().Be(2000);
            result[0].WoodPrice.Should().Be(9);
            result[0].Stone.Should().Be(3000);
            result[0].StonePrice.Should().Be(8);
            result[0].Ore.Should().Be(4000);
            result[0].OrePrice.Should().Be(7);

            result[1].Id.Should().Be(2);
            result[1].Date.Should().BeCloseTo(DateTime.UtcNow, 1000);
            result[1].Ore.Should().Be(25000);
            result[1].OrePrice.Should().Be(4);
        }
    }
}