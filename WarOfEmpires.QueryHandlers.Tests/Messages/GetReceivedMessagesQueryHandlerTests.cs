using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using System;
using System.Linq;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Messages;
using WarOfEmpires.QueryHandlers.Messages;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Messages {
    [TestClass]
    public sealed class GetReceivedMessagesQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();

        public object Date { get; private set; }

        public Player AddPlayer(int id, string email, string displayName, UserStatus status) {
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

            return player;
        }

        [TestMethod]
        public void GetReceivedMessagesQueryHandler_Returns_All_Received_Messages() {
            var handler = new GetReceivedMessagesQueryHandler(_context);
            var query = new GetReceivedMessagesQuery("test@test.com");
            var recipient = AddPlayer(1, "test@test.com", "Test", UserStatus.Active);
            var senders = new[] {
                AddPlayer(2, "active1@test.com", "Test", UserStatus.Active),
                AddPlayer(3, "active2@test.com", "Test", UserStatus.Active),
                AddPlayer(4, "inactive@test.com", "Test", UserStatus.Inactive),
            };

            recipient.ReceivedMessages.Returns(senders.Select(s => new Message(s, recipient, "Test", "Test")).ToList());

            var result = handler.Execute(query);

            result.Count().Should().Be(3);
        }

        [TestMethod]
        public void GetReceivedMessagesQueryHandler_Returns_Correct_Message_Data() {
            var handler = new GetReceivedMessagesQueryHandler(_context);
            var query = new GetReceivedMessagesQuery("test@test.com");
            var recipient = AddPlayer(1, "test@test.com", "Recipient", UserStatus.Active);
            var sender = AddPlayer(2, "sender@test.com", "Sender", UserStatus.Active);

            recipient.ReceivedMessages.Returns(new List<Message>() { new Message(sender, recipient, "Testsubject", "Testbody") });

            var result = handler.Execute(query);
            var message = result.FirstOrDefault();

            message.Should().NotBeNull();
            message.Sender.Should().Be("Sender");
            message.Date.Should().BeCloseTo(DateTime.UtcNow);
            message.IsRead.Should().BeFalse();
            message.Subject.Should().Be("Testsubject");
        }
    }
}