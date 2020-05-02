using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Players {
    [TestClass]
    public sealed class GetPlayerIsInAllianceQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly Alliance _alliance;
        private readonly Player _player;

        public GetPlayerIsInAllianceQueryHandlerTests() {
            _alliance = Substitute.For<Alliance>();
            _player = Substitute.For<Player>();

            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            _player.User.Returns(user);
            _context.Players.Add(_player);
        }

        [TestMethod]
        public void GetPlayerIsInAllianceQueryHandler_Returns_True_For_In_Alliance() {
            _player.Alliance.Returns(_alliance);

            var handler = new GetPlayerIsInAllianceQueryHandler(_context);
            var query = new GetPlayerIsInAllianceQuery("test@test.com");

            var result = handler.Execute(query);

            result.Should().BeTrue();
        }

        [TestMethod]
        public void GetPlayerIsInAllianceQueryHandler_Returns_False_For_Not_In_Alliance() {
            _player.Alliance.Returns((Alliance)null);

            var handler = new GetPlayerIsInAllianceQueryHandler(_context);
            var query = new GetPlayerIsInAllianceQuery("test@test.com");
            
            var result = handler.Execute(query);

            result.Should().BeFalse();
        }
    }
}