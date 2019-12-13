using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Attacks;
using WarOfEmpires.QueryHandlers.Attacks;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Players {
    [TestClass]
    public sealed class GetDefenderQueryHandlerTests {
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
            player.Farmers.Returns(1);
            player.WoodWorkers.Returns(2);
            player.StoneMasons.Returns(3);
            player.OreMiners.Returns(4);
            player.Peasants.Returns(5);
            player.Archers.Returns(new Troops(15, 5));
            player.Cavalry.Returns(new Troops(3, 1));
            player.Footmen.Returns(new Troops(3, 1));

            _context.Users.Add(user);
            _context.Players.Add(player);
        }

        [TestMethod]
        public void GetDefenderQueryHandler_Returns_Correct_Information() {
            var handler = new GetDefenderQueryHandler(_context);
            var query = new GetDefenderQuery("2");

            AddPlayer(1, "test1@test.com", "Test display name 1", UserStatus.Active);
            AddPlayer(2, "test2@test.com", "Test display name 2", UserStatus.Active);

            var results = handler.Execute(query);

            results.DefenderId.Should().Be(2);
            results.DisplayName.Should().Be("Test display name 2");
            results.Population.Should().Be(43);
        }

        [TestMethod]
        public void GetDefenderQueryHandler_Throws_Exception_For_Alphanumeric_Id() {
            var handler = new GetDefenderQueryHandler(_context);
            var query = new GetDefenderQuery("A");

            AddPlayer(1, "test1@test.com", "Test display name 1", UserStatus.Active);

            Action action = () => handler.Execute(query);

            action.Should().Throw<FormatException>();
        }

        [TestMethod]
        public void GetDefenderQueryHandler_Throws_Exception_For_Nonexistent_Id() {
            var handler = new GetDefenderQueryHandler(_context);
            var query = new GetDefenderQuery("5");

            AddPlayer(1, "test1@test.com", "Test display name 1", UserStatus.Active);

            Action action = () => handler.Execute(query);

            action.Should().Throw<InvalidOperationException>();
        }
    }
}