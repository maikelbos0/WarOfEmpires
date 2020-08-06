using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Markets;
using WarOfEmpires.QueryHandlers.Markets;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Tests.Markets {
    [TestClass]
    public sealed class GetMarketQueryHandlerTests {
        [TestMethod]
        public void GetMarketQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithBuilding(BuildingType.Market, 3)
                .WithWorkers(WorkerType.Merchants, 7)
                .WithCaravan(1, new Merchandise(MerchandiseType.Wood, 25000, 10))
                .WithCaravan(2, new Merchandise(MerchandiseType.Ore, 25000, 10));

            var sellerId = 2;
            var caravanId = 3;

            foreach (var status in new[] { UserStatus.Active, UserStatus.Active, UserStatus.Active, UserStatus.Active, UserStatus.Inactive, UserStatus.New }) {
                builder
                    .BuildPlayer(sellerId++, status: status)
                    .WithCaravan(caravanId++, new Merchandise(MerchandiseType.Food, 10000, 10), new Merchandise(MerchandiseType.Wood, 11000, 9), new Merchandise(MerchandiseType.Stone, 12000, 8), new Merchandise(MerchandiseType.Ore, 13000, 7))
                    .WithCaravan(caravanId++, new Merchandise(MerchandiseType.Food, 5000, 5), new Merchandise(MerchandiseType.Wood, 6000, 4), new Merchandise(MerchandiseType.Stone, 7000, 3), new Merchandise(MerchandiseType.Ore, 8000, 2));
            }

            var handler = new GetMarketQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetMarketQuery("test1@test.com");

            var result = handler.Execute(query);

            result.TotalMerchants.Should().Be(7);
            result.AvailableMerchants.Should().Be(5);
            result.CaravanCapacity.Should().Be(25000);
            result.AvailableCapacity.Should().Be(125000);
            result.Merchandise.Should().HaveCount(4);

            var food = result.Merchandise.Single(m => m.Type == "Food");

            food.Name.Should().Be("Food");
            food.LowestPrice.Should().Be(5);
            food.AvailableAtLowestPrice.Should().Be(20000);
            food.TotalAvailable.Should().Be(60000);

            var wood = result.Merchandise.Single(m => m.Type == "Wood");

            wood.Name.Should().Be("Wood");
            wood.LowestPrice.Should().Be(4);
            wood.AvailableAtLowestPrice.Should().Be(24000);
            wood.TotalAvailable.Should().Be(93000);

            var stone = result.Merchandise.Single(m => m.Type == "Stone");

            stone.Name.Should().Be("Stone");
            stone.LowestPrice.Should().Be(3);
            stone.AvailableAtLowestPrice.Should().Be(28000);
            stone.TotalAvailable.Should().Be(76000);

            var ore = result.Merchandise.Single(m => m.Type == "Ore");

            ore.Name.Should().Be("Ore");
            ore.LowestPrice.Should().Be(2);
            ore.AvailableAtLowestPrice.Should().Be(32000);
            ore.TotalAvailable.Should().Be(109000);
        }
    }
}