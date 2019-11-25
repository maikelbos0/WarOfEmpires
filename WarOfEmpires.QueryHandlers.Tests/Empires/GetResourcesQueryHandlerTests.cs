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
    public sealed class GetResourcesQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();

        public GetResourcesQueryHandlerTests() {
            var user = Substitute.For<User>();
            var player = Substitute.For<Player>();

            user.Id.Returns(1);
            user.Status.Returns(UserStatus.Active);
            user.Email.Returns("test@test.com");

            player.User.Returns(user);
            player.Resources.Returns(new Resources(12000, 1500, 2000, 500, 1000));

            _context.Users.Add(user);
            _context.Players.Add(player);
        }

        [TestMethod]
        public void GetResourcesQueryHandler_Returns_Correct_Resources() {
            var query = new GetResourcesQuery("test@test.com");
            var handler = new GetResourcesQueryHandler(_context);

            var result = handler.Execute(query);

            result.Gold.Should().Be(12000);
            result.Food.Should().Be(1500);
            result.Wood.Should().Be(2000);
            result.Stone.Should().Be(500);
            result.Ore.Should().Be(1000);
        }
    }
}