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
    public sealed class GetResourceHeaderQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly ResourcesMap _resourcesMap = new ResourcesMap();

        public GetResourceHeaderQueryHandlerTests() {
            var user = Substitute.For<User>();
            var player = Substitute.For<Player>();

            user.Id.Returns(1);
            user.Status.Returns(UserStatus.Active);
            user.Email.Returns("test@test.com");

            player.User.Returns(user);
            player.Resources.Returns(new Resources(12000, 1500, 2000, 500, 1000));
            player.AttackTurns.Returns(55);

            _context.Users.Add(user);
            _context.Players.Add(player);
        }

        [TestMethod]
        public void GetResourceHeaderQueryHandler_Returns_Correct_Resources() {
            var query = new GetResourceHeaderQuery("test@test.com");
            var handler = new GetResourceHeaderQueryHandler(_context, _resourcesMap);

            var result = handler.Execute(query);

            result.Resources.Gold.Should().Be(12000);
            result.Resources.Food.Should().Be(1500);
            result.Resources.Wood.Should().Be(2000);
            result.Resources.Stone.Should().Be(500);
            result.Resources.Ore.Should().Be(1000);
            result.AttackTurns.Should().Be(55);
        }
    }
}