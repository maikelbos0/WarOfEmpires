using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Alliances;
using WarOfEmpires.QueryHandlers.Common;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Alliances {
    [TestClass]
    public class GetBankedResourcesQueryHandlerTests {
        [TestMethod]
        public void GetBankedResourcesQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1);

            builder.Alliance.BankedResources.Returns(new Resources(5000, 4000, 3000, 2000, 1000));

            var handler = new GetBankedResourcesQueryHandler(builder.Context, new ResourcesMap());
            var query = new GetBankedResourcesQuery("test1@test.com");

            var result = handler.Execute(query);

            result.BankedResources.Gold.Should().Be(5000);
            result.BankedResources.Food.Should().Be(4000);
            result.BankedResources.Wood.Should().Be(3000);
            result.BankedResources.Stone.Should().Be(2000);
            result.BankedResources.Ore.Should().Be(1000);

            result.BankTurns.Should().Be(12);
        }
    }
}
