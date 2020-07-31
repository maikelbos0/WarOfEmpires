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
    public sealed class GetResourceHeaderQueryHandlerTests {
        [TestMethod]
        public void GetResourceHeaderQueryHandler_Returns_Correct_Resources() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            builder.Player.Resources.Returns(new Resources(12000, 1500, 2000, 500, 1000));
            builder.Player.BankedResources.Returns(new Resources(40000, 100, 20000, 500, 2000));
            builder.Player.AttackTurns.Returns(55);
            builder.Player.BankTurns.Returns(5);

            var handler = new GetResourceHeaderQueryHandler(builder.Context, new ResourcesMap());
            var query = new GetResourceHeaderQuery("test1@test.com");

            var result = handler.Execute(query);

            result.Resources.Gold.Should().Be(12000);
            result.Resources.Food.Should().Be(1500);
            result.Resources.Wood.Should().Be(2000);
            result.Resources.Stone.Should().Be(500);
            result.Resources.Ore.Should().Be(1000);

            result.BankedResources.Gold.Should().Be(40000);
            result.BankedResources.Food.Should().Be(100);
            result.BankedResources.Wood.Should().Be(20000);
            result.BankedResources.Stone.Should().Be(500);
            result.BankedResources.Ore.Should().Be(2000);

            result.AttackTurns.Should().Be(55);
            result.BankTurns.Should().Be(5);
        }
    }
}