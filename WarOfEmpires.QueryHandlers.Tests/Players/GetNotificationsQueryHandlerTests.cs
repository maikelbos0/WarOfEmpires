using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using WarOfEmpires.Domain.Alliances;
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
        /* TODO use fake builder */
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly Player _player;
        private readonly Message _message;
        private readonly Attack _attack;
        private readonly Invite _invite;

        public GetNotificationsQueryHandlerTests() {
            var user = Substitute.For<User>();

            user.Status.Returns(UserStatus.Active);
            user.Email.Returns("test@test.com");

            _player = Substitute.For<Player>();
            _player.User.Returns(user);
            _player.GetUpkeepPerTurn().Returns(new Resources(gold: 1000, food: 100));
            _player.GetTotalResources().Returns(new Resources(gold: 30000, food: 1000));
            _player.GetResourcesPerTurn().Returns(new Resources(gold: 1000, food: 100));

            _context.Users.Add(user);
            _context.Players.Add(_player);

            _message = Substitute.For<Message>();
            _player.ReceivedMessages.Returns(new List<Message>() { _message });

            _attack = Substitute.For<Attack>();
            _player.ReceivedAttacks.Returns(new List<Attack>() { _attack });

            _invite = Substitute.For<Invite>();
            _player.Invites.Returns(new List<Invite>() { _invite });
        }

        [TestMethod]
        public void GetNotificationsQueryHandler_HasNewMessages_Is_True_For_Unread_Messages() {
            _message.IsRead.Returns(false);

            var handler = new GetNotificationsQueryHandler(_context);
            var query = new GetNotificationsQuery("test@test.com");

            var result = handler.Execute(query);

            result.HasNewMessages.Should().BeTrue();
        }

        [TestMethod]
        public void GetNotificationsQueryHandler_HasNewMessages_Is_False_For_Read_Messages() {
            _message.IsRead.Returns(true);

            var handler = new GetNotificationsQueryHandler(_context);
            var query = new GetNotificationsQuery("test@test.com");

            var result = handler.Execute(query);

            result.HasNewMessages.Should().BeFalse();
        }

        [TestMethod]
        public void GetNotificationsQueryHandler_HasNewAttacks_Is_True_For_Unread_Attacks() {
            _attack.IsRead.Returns(false);

            var handler = new GetNotificationsQueryHandler(_context);
            var query = new GetNotificationsQuery("test@test.com");

            var result = handler.Execute(query);

            result.HasNewAttacks.Should().BeTrue();
        }

        [TestMethod]
        public void GetNotificationsQueryHandler_HasNewAttacks_Is_False_For_Read_Attacks() {
            _attack.IsRead.Returns(true);

            var handler = new GetNotificationsQueryHandler(_context);
            var query = new GetNotificationsQuery("test@test.com");

            var result = handler.Execute(query);

            result.HasNewAttacks.Should().BeFalse();
        }

        [TestMethod]
        public void GetNotificationsQueryHandler_HasNewSales_Is_True_For_Unread_Sales() {
            _player.HasNewMarketSales.Returns(true);

            var handler = new GetNotificationsQueryHandler(_context);
            var query = new GetNotificationsQuery("test@test.com");

            var result = handler.Execute(query);

            result.HasNewMarketSales.Should().BeTrue();
        }

        [TestMethod]
        public void GetNotificationsQueryHandler_HasNewSales_Is_False_For_Read_Sales() {
            _player.HasNewMarketSales.Returns(false);

            var handler = new GetNotificationsQueryHandler(_context);
            var query = new GetNotificationsQuery("test@test.com");

            var result = handler.Execute(query);

            result.HasNewMarketSales.Should().BeFalse();
        }

        [TestMethod]
        public void GetNotificationsQueryHandler_HasHousingShortate_Is_True_For_Shortage() {
            _player.GetAvailableHousingCapacity().Returns(4);
            _player.GetTheoreticalRecruitsPerDay().Returns(5);

            var handler = new GetNotificationsQueryHandler(_context);
            var query = new GetNotificationsQuery("test@test.com");

            var result = handler.Execute(query);

            result.HasHousingShortage.Should().BeTrue();
        }

        [TestMethod]
        public void GetNotificationsQueryHandler_HasHousingShortate_Is_False_For_Enough_Room() {
            _player.GetAvailableHousingCapacity().Returns(5);
            _player.GetTheoreticalRecruitsPerDay().Returns(5);

            var handler = new GetNotificationsQueryHandler(_context);
            var query = new GetNotificationsQuery("test@test.com");

            var result = handler.Execute(query);

            result.HasHousingShortage.Should().BeFalse();
        }

        [TestMethod]
        public void GetNotificationsQueryHandler_HasUpkeepShortage_Is_True_If_Gold_Runs_Out_In_8_Hours() {
            _player.GetResourcesPerTurn().Returns(new Resources(gold: 200, food: 100));

            var handler = new GetNotificationsQueryHandler(_context);
            var query = new GetNotificationsQuery("test@test.com");

            var result = handler.Execute(query);

            result.HasUpkeepShortage.Should().BeTrue();
        }

        [TestMethod]
        public void GetNotificationsQueryHandler_HasUpkeepShortage_Is_True_If_Food_Runs_Out_In_8_Hours() {
            _player.GetResourcesPerTurn().Returns(new Resources(gold: 1000, food: 20));

            var handler = new GetNotificationsQueryHandler(_context);
            var query = new GetNotificationsQuery("test@test.com");

            var result = handler.Execute(query);

            result.HasUpkeepShortage.Should().BeTrue();
        }

        [TestMethod]
        public void GetNotificationsQueryHandler_HasUpkeepShortage_Is_False_If_Enough_Resources() {
            _player.GetResourcesPerTurn().Returns(new Resources(gold: 1000, food: 100));

            var handler = new GetNotificationsQueryHandler(_context);
            var query = new GetNotificationsQuery("test@test.com");

            var result = handler.Execute(query);

            result.HasUpkeepShortage.Should().BeFalse();
        }

        [TestMethod]
        public void GetNotificationsQueryHandler_HasSoldierShortage_Is_True_If_A_Troop_Penalty_Exists() {
            _player.GetSoldierRecruitsPenalty().Returns(1);

            var handler = new GetNotificationsQueryHandler(_context);
            var query = new GetNotificationsQuery("test@test.com");

            var result = handler.Execute(query);

            result.HasSoldierShortage.Should().BeTrue();
        }

        [TestMethod]
        public void GetNotificationsQueryHandler_HasSoldierShortage_Is_False_A_Troop_Penalty_Does_Not_Exist() {
            _player.GetSoldierRecruitsPenalty().Returns(0);

            var handler = new GetNotificationsQueryHandler(_context);
            var query = new GetNotificationsQuery("test@test.com");

            var result = handler.Execute(query);

            result.HasSoldierShortage.Should().BeFalse();
        }

        [TestMethod]
        public void GetNotificationsQueryHandler_HasNewInvites_Is_True_For_Unread_Invites() {
            _invite.IsRead.Returns(false);

            var handler = new GetNotificationsQueryHandler(_context);
            var query = new GetNotificationsQuery("test@test.com");

            var result = handler.Execute(query);

            result.HasNewInvites.Should().BeTrue();
        }

        [TestMethod]
        public void GetNotificationsQueryHandler_HasNewInvites_Is_False_For_Read_Invites() {
            _invite.IsRead.Returns(true);

            var handler = new GetNotificationsQueryHandler(_context);
            var query = new GetNotificationsQuery("test@test.com");

            var result = handler.Execute(query);

            result.HasNewInvites.Should().BeFalse();
        }
    }
}