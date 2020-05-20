using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Alliances;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Alliances {
    [TestClass]
    public sealed class GetInvitePlayerQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();

        public GetInvitePlayerQueryHandlerTests() {
            var user = Substitute.For<User>();
            var player = Substitute.For<Player>();

            user.Id.Returns(1);
            user.Status.Returns(UserStatus.Active);

            player.User.Returns(user);
            player.Id.Returns(1);
            player.DisplayName.Returns("A name");

            _context.Users.Add(user);
            _context.Players.Add(player);
        }

        [TestMethod]
        public void GetInvitePlayerQueryHandler_Returns_Correct_Information() {
            var handler = new GetInvitePlayerQueryHandler(_context);
            var query = new GetInvitePlayerQuery("1");

            var result = handler.Execute(query);

            result.PlayerId.Should().Be("1");
            result.PlayerName.Should().Be("A name");
        }
    }
}