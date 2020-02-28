using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Players {
    [TestClass]
    public sealed class GetPlayerDetailsQueryHandlerTests {
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
            player.Peasants.Returns(5);
            player.Workers.Returns(new List<Workers>() {
                new Workers(WorkerType.Farmer, 1),
                new Workers(WorkerType.WoodWorker, 2),
                new Workers(WorkerType.StoneMason, 3),
                new Workers(WorkerType.OreMiner, 4),
                new Workers(WorkerType.SiegeEngineer, 6)
            });
            player.Troops.Returns(new List<Troops>() {
                new Troops(TroopType.Archers, 15, 5),
                new Troops(TroopType.Cavalry, 3, 1),
                new Troops(TroopType.Footmen, 3, 1)
            });

            _context.Users.Add(user);
            _context.Players.Add(player);
        }

        [TestMethod]
        public void GetPlayerDetailsQueryHandler_Returns_Correct_Information() {
            var handler = new GetPlayerDetailsQueryHandler(_context);
            var query = new GetPlayerDetailsQuery("2");

            AddPlayer(1, "test1@test.com", "Test display name 1", UserStatus.Active);
            AddPlayer(2, "test2@test.com", "Test display name 2", UserStatus.Active);

            var result = handler.Execute(query);

            result.Id.Should().Be(2);
            result.DisplayName.Should().Be("Test display name 2");
            result.Population.Should().Be(49);
        }

        [TestMethod]
        public void GetPlayerDetailsQueryHandler_Throws_Exception_For_Alphanumeric_Id() {
            var handler = new GetPlayerDetailsQueryHandler(_context);
            var query = new GetPlayerDetailsQuery("A");

            AddPlayer(1, "test1@test.com", "Test display name 1", UserStatus.Active);

            Action action = () => handler.Execute(query);

            action.Should().Throw<FormatException>();
        }

        [TestMethod]
        public void GetPlayerDetailsQueryHandler_Throws_Exception_For_Nonexistent_Id() {
            var handler = new GetPlayerDetailsQueryHandler(_context);
            var query = new GetPlayerDetailsQuery("5");

            AddPlayer(1, "test1@test.com", "Test display name 1", UserStatus.Active);

            Action action = () => handler.Execute(query);

            action.Should().Throw<InvalidOperationException>();
        }
    }
}