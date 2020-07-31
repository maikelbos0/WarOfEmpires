using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Common;
using WarOfEmpires.QueryHandlers.Empires;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Empires {
    [TestClass]
    public sealed class GetBankedResourcesQueryHandlerTests {
        [TestMethod]
        public void GetBankedResourcesQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder().BuildPlayer(1);

            builder.Player.BankedResources.Returns(new Resources(5000, 4000, 3000, 2000, 1000));
            builder.Player.GetBankCapacity().Returns(new Resources(50000, 40000, 30000, 20000, 10000));
            builder.Player.GetAvailableBankCapacity().Returns(new Resources(45000, 36000, 27000, 18000, 9000));
            builder.Player.GetBankableResources().Returns(new Resources(1000, 2000, 3000, 4000, 5000));

            var handler = new GetBankedResourcesQueryHandler(builder.Context, new ResourcesMap());
            var query = new GetBankedResourcesQuery("test1@test.com");

            var result = handler.Execute(query);

            result.BankedResources.Gold.Should().Be(5000);
            result.BankedResources.Food.Should().Be(4000);
            result.BankedResources.Wood.Should().Be(3000);
            result.BankedResources.Stone.Should().Be(2000);
            result.BankedResources.Ore.Should().Be(1000);

            result.Capacity.Gold.Should().Be(50000);
            result.Capacity.Food.Should().Be(40000);
            result.Capacity.Wood.Should().Be(30000);
            result.Capacity.Stone.Should().Be(20000);
            result.Capacity.Ore.Should().Be(10000);

            result.AvailableCapacity.Gold.Should().Be(45000);
            result.AvailableCapacity.Food.Should().Be(36000);
            result.AvailableCapacity.Wood.Should().Be(27000);
            result.AvailableCapacity.Stone.Should().Be(18000);
            result.AvailableCapacity.Ore.Should().Be(9000);

            result.BankableResources.Gold.Should().Be(1000);
            result.BankableResources.Food.Should().Be(2000);
            result.BankableResources.Wood.Should().Be(3000);
            result.BankableResources.Stone.Should().Be(4000);
            result.BankableResources.Ore.Should().Be(5000);
        }
    }
}