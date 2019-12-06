using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Players {
    [TestClass]
    public sealed class GetNotificationsQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly Message _message;

        public GetNotificationsQueryHandlerTests() {
            var user = Substitute.For<User>();
            var player = Substitute.For<Player>();

            user.Status.Returns(UserStatus.Active);
            user.Email.Returns("test@test.com");

            player.User.Returns(user);

            _context.Users.Add(user);
            _context.Players.Add(player);

            _message = Substitute.For<Message>();
            player.ReceivedMessages.Returns(new List<Message>() { _message });
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
    }
}