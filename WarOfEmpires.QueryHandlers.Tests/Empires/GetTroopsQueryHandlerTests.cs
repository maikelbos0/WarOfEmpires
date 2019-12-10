using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
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
            player.Archers.Returns(7);
            player.MercenaryArchers.Returns(6);
            player.Cavalry.Returns(5);
            player.MercenaryCavalry.Returns(4);
            player.Footmen.Returns(3);
            player.MercenaryFootmen.Returns(2);

            _context.Users.Add(user);
            _context.Players.Add(player);
        }

        [TestMethod]
        // this method is also in the GetWorkersQueryHandlerTests
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

        // add tests for troop strength if/when implemented
        // upkeep is handled on the worker's pages
    }
}
