using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Empires;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Empires {
    [TestClass]
    public sealed class GetTroopsQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();

        public GetTroopsQueryHandlerTests() {
            var user = Substitute.For<User>();
            var player = Substitute.For<Player>();

            user.Id.Returns(1);
            user.Status.Returns(UserStatus.Active);
            user.Email.Returns("test@test.com");

            player.User.Returns(user);
            player.Id.Returns(1);
            player.Peasants.Returns(1);
            player.Archers.Returns(new Troops(7, 6));
            player.Cavalry.Returns(new Troops(5, 4));
            player.Footmen.Returns(new Troops(3, 2));
            player.GetUpkeepPerTurn().Returns(new Resources(gold: 500, food: 30));
            player.GetTotalResources().Returns(new Resources(gold: 1000, food: 100));
            player.GetResourcesPerTurn().Returns(new Resources(gold: 400, food: 20));
            player.HasUpkeepRunOut.Returns(true);

            _context.Users.Add(user);
            _context.Players.Add(player);
        }

        [TestMethod]
        public void GetTroopsQueryHandler_Returns_Correct_Peasants() {
            var query = new GetTroopsQuery("test@test.com");
            var handler = new GetTroopsQueryHandler(_context);

            var result = handler.Execute(query);

            result.CurrentPeasants.Should().Be(1);
        }

        [TestMethod]
        public void GetTroopsQueryHandler_Returns_Correct_Troops() {
            var query = new GetTroopsQuery("test@test.com");
            var handler = new GetTroopsQueryHandler(_context);

            var result = handler.Execute(query);

            result.CurrentArchers.Should().Be(7);
            result.CurrentMercenaryArchers.Should().Be(6);
            result.CurrentCavalry.Should().Be(5);
            result.CurrentMercenaryCavalry.Should().Be(4);
            result.CurrentFootmen.Should().Be(3);
            result.CurrentMercenaryFootmen.Should().Be(2);
        }

        [TestMethod]
        public void GetTroopsQueryHandler_Returns_Correct_Additional_Information() {
            var query = new GetTroopsQuery("test@test.com");
            var handler = new GetTroopsQueryHandler(_context);

            var result = handler.Execute(query);

            result.WillUpkeepRunOut.Should().BeTrue();
            result.HasUpkeepRunOut.Should().BeTrue();
        }

        // TODO add tests for troop strength if/when implemented
    }
}
