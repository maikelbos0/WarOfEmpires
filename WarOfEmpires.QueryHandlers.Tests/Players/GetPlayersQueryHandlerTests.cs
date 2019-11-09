using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Linq;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Players {
    [TestClass]
    public sealed class GetPlayersQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();

        public void AddPlayer(int id, string email, string displayName, UserStatus status) {
            var user = Substitute.For<User>();
            var player = Substitute.For<Player>();

            user.Id.Returns(id);
            user.Status.Returns(status);
            user.Email.Returns(email);

            player.User.Returns(user);
            player.Id.Returns(id);
            player.DisplayName.Returns(displayName);

            _context.Users.Add(user);
            _context.Players.Add(player);
        }

        [TestMethod]
        public void GetPlayersQueryHandler_Returns_All_Active_Players() {
            for (var i = 0; i < 9; i++) {
                AddPlayer(i+1, $"test{i}@test.com", $"Test {i}", (UserStatus)(i % 3 + 1));
            }

            var handler = new GetPlayersQueryHandler(_context);
            var query = new GetPlayersQuery();

            var results = handler.Execute(query);

            results.Should().HaveCount(3);
        }

        [TestMethod]
        public void GetPlayersQueryHandler_Returns_Id() {
            var handler = new GetPlayersQueryHandler(_context);
            var query = new GetPlayersQuery();

            AddPlayer(1, "test@test.com", "Test", UserStatus.Active);

            var results = handler.Execute(query);

            results.Single().Id.Should().Be(1);
        }

        [TestMethod]
        public void GetPlayersQueryHandler_Returns_DisplayName() {
            var handler = new GetPlayersQueryHandler(_context);
            var query = new GetPlayersQuery();

            AddPlayer(1, "test@test.com", "Test display name", UserStatus.Active);

            var results = handler.Execute(query);

            results.Single().DisplayName.Should().Be("Test display name");
        }
    }
}