using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Messages;
using WarOfEmpires.QueryHandlers.Messages;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Messages {
    [TestClass]
    public sealed class GetSentMessagesQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();

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
        public void GetSentMessagesQueryHandler_Returns_All_Sent_Messages() {
            var handler = new GetSentMessagesQueryHandler(_context);
            var query = new GetSentMessagesQuery("test@test.com");
            var sender = AddPlayer(1, "test@test.com", "Test", UserStatus.Active);
            var recipients = new[] {
                AddPlayer(2, "active1@test.com", "Test", UserStatus.Active),
                AddPlayer(3, "active2@test.com", "Test", UserStatus.Active),
                AddPlayer(4, "inactive@test.com", "Test", UserStatus.Inactive),
            };

            sender.SentMessages.Returns(recipients.Select(r => new Message(sender, r, "Test", "Test")).ToList());

            var result = handler.Execute(query);

            result.Count().Should().Be(3);
        }

        [TestMethod]
        public void GetSentMessagesQueryHandler_Returns_Correct_Message_Data() {
            var handler = new GetSentMessagesQueryHandler(_context);
            var query = new GetSentMessagesQuery("test@test.com");
            var sender = AddPlayer(2, "test@test.com", "Sender", UserStatus.Active);
            var recipient = AddPlayer(1, "recipient@test.com", "Recipient", UserStatus.Active);

            sender.SentMessages.Returns(new List<Message>() { new Message(sender, recipient, "Testsubject", "Testbody") });

            var result = handler.Execute(query);
            var message = result.FirstOrDefault();

            message.Should().NotBeNull();
            message.Recipient.Should().Be("Recipient");
            message.Date.Should().BeCloseTo(DateTime.UtcNow, 1000);
            message.IsRead.Should().BeFalse();
            message.Subject.Should().Be("Testsubject");
        }
    }
}