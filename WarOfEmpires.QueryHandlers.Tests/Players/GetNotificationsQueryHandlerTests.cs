using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Players {
    [TestClass]
    public sealed class GetNotificationsQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly Player _player;
        private readonly Message _message;
        private readonly Attack _attack;

        public GetNotificationsQueryHandlerTests() {
            var user = Substitute.For<User>();

            user.Status.Returns(UserStatus.Active);
            user.Email.Returns("test@test.com");

            _player = Substitute.For<Player>();
            _player.User.Returns(user);
            _player.GetUpkeepPerTurn().Returns(new Resources(gold: 1000, food: 100));
            _player.Resources.Returns(new Resources(gold: 30000, food: 1000));
            _player.GetResourcesPerTurn().Returns(new Resources(gold: 1000, food: 100));

            _context.Users.Add(user);
            _context.Players.Add(_player);

            _message = Substitute.For<Message>();
            _player.ReceivedMessages.Returns(new List<Message>() { _message });

            _attack = Substitute.For<Attack>();
            _player.ReceivedAttacks.Returns(new List<Attack>() { _attack });
        }

        [TestMethod]
        public void GetNotificationsQueryHandler_HasNewMessages_Is_True_For_Unread_Messages() {
            _message.IsRead.Returns(false);

            var handler = new GetNotificationsQueryHandler(_context);
            var query = new GetNotificationsQuery("test@test.com");
            
            var results = handler.Execute(query);

            results.HasNewMessages.Should().BeTrue();
        }

        [TestMethod]
        public void GetNotificationsQueryHandler_HasNewMessages_Is_False_For_Read_Messages() {
            _message.IsRead.Returns(true);

            var handler = new GetNotificationsQueryHandler(_context);
            var query = new GetNotificationsQuery("test@test.com");

            var results = handler.Execute(query);

            results.HasNewMessages.Should().BeFalse();
        }

        [TestMethod]
        public void GetNotificationsQueryHandler_HasNewAttacks_Is_True_For_Unread_Attacks() {
            _attack.IsRead.Returns(false);

            var handler = new GetNotificationsQueryHandler(_context);
            var query = new GetNotificationsQuery("test@test.com");

            var results = handler.Execute(query);

            results.HasNewAttacks.Should().BeTrue();
        }

        [TestMethod]
        public void GetNotificationsQueryHandler_HasNewAttacks_Is_False_For_Read_Attacks() {
            _attack.IsRead.Returns(true);

            var handler = new GetNotificationsQueryHandler(_context);
            var query = new GetNotificationsQuery("test@test.com");

            var results = handler.Execute(query);

            results.HasNewAttacks.Should().BeFalse();
        }

        [TestMethod]
        public void GetNotificationsQueryHandler_HasHousingShortate_Is_True_For_Shortage() {
            _player.GetAvailableHousingCapacity().Returns(4);
            _player.GetTheoreticalRecruitsPerDay().Returns(5);

            var handler = new GetNotificationsQueryHandler(_context);
            var query = new GetNotificationsQuery("test@test.com");

            var results = handler.Execute(query);

            results.HasHousingShortage.Should().BeTrue();
        }

        [TestMethod]
        public void GetNotificationsQueryHandler_HasHousingShortate_Is_False_For_Enough_Room() {
            _player.GetAvailableHousingCapacity().Returns(5);
            _player.GetTheoreticalRecruitsPerDay().Returns(5);

            var handler = new GetNotificationsQueryHandler(_context);
            var query = new GetNotificationsQuery("test@test.com");

            var results = handler.Execute(query);

            results.HasHousingShortage.Should().BeFalse();
        }

        [TestMethod]
        public void GetNotificationsQueryHandler_HasUpkeepShortage_Is_True_If_Gold_Runs_Out_In_8_Hours() {
            _player.GetResourcesPerTurn().Returns(new Resources(gold: 200, food: 100));

            var handler = new GetNotificationsQueryHandler(_context);
            var query = new GetNotificationsQuery("test@test.com");

            var results = handler.Execute(query);

            results.HasUpkeepShortage.Should().BeTrue();
        }

        [TestMethod]
        public void GetNotificationsQueryHandler_HasUpkeepShortage_Is_True_If_Food_Runs_Out_In_8_Hours() {
            _player.GetResourcesPerTurn().Returns(new Resources(gold: 1000, food: 20));

            var handler = new GetNotificationsQueryHandler(_context);
            var query = new GetNotificationsQuery("test@test.com");

            var results = handler.Execute(query);

            results.HasUpkeepShortage.Should().BeTrue();
        }

        [TestMethod]
        public void GetNotificationsQueryHandler_HasUpkeepShortage_Is_False_If_Enough_Resources() {
            _player.GetResourcesPerTurn().Returns(new Resources(gold: 1000, food: 100));

            var handler = new GetNotificationsQueryHandler(_context);
            var query = new GetNotificationsQuery("test@test.com");

            var results = handler.Execute(query);

            results.HasUpkeepShortage.Should().BeFalse();
        }
    }
}