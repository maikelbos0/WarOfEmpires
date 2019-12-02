using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Messages;
using WarOfEmpires.QueryHandlers.Messages;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Messages {
    [TestClass]
    public sealed class GetReceivedMessageQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();

        public GetReceivedMessageQueryHandlerTests() {
            var senderUser = Substitute.For<User>();
            senderUser.Email.Returns("sender@test.com");
            senderUser.Status.Returns(UserStatus.Active);

            var message = Substitute.For<Message>();

            var sender = Substitute.For<Player>();
            sender.ReceivedMessages.Returns(new List<Message>());
            sender.SentMessages.Returns(new List<Message>() { message });
            sender.User.Returns(senderUser);
            sender.DisplayName.Returns("Sender test");

            _context.Users.Add(senderUser);
            _context.Players.Add(sender);

            var recipientUser = Substitute.For<User>();
            recipientUser.Email.Returns("recipient@test.com");
            recipientUser.Status.Returns(UserStatus.Active);

            var recipient = Substitute.For<Player>();
            recipient.ReceivedMessages.Returns(new List<Message>() { message });
            recipient.SentMessages.Returns(new List<Message>());
            recipient.User.Returns(recipientUser);

            message.Id.Returns(1);
            message.Sender.Returns(sender);
            message.Recipient.Returns(recipient);
            message.Subject.Returns("Subject test");
            message.Body.Returns("Body test");
            message.Date.Returns(new DateTime(2019, 1, 1));
            message.IsRead.Returns(false);

            _context.Users.Add(recipientUser);
            _context.Players.Add(recipient);
        }

        [TestMethod]
        public void GetReceivedMessageQueryHandler_Returns_Correct_Information() {
            var handler = new GetReceivedMessageQueryHandler(_context);
            var query = new GetReceivedMessageQuery("recipient@test.com", "1");

            var result = handler.Execute(query);

            result.Subject.Should().Be("Subject test");
            result.Body.Should().Be("Body test");
            result.Sender.Should().Be("Sender test");
            result.Date.Should().Be(new DateTime(2019, 1, 1));
            result.IsRead.Should().BeFalse();
        }

        [TestMethod]
        public void GetReceivedMessageQueryHandler_Throws_Exception_For_Message_Received_By_Different_Player() {
            var handler = new GetReceivedMessageQueryHandler(_context);
            var query = new GetReceivedMessageQuery("sender@test.com", "1");

            Action action = () => handler.Execute(query);

            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void GetReceivedMessageQueryHandler_Throws_Exception_For_Alphanumeric_MessageId() {
            var handler = new GetReceivedMessageQueryHandler(_context);
            var query = new GetReceivedMessageQuery("recipient@test.com", "A");

            Action action = () => handler.Execute(query);

            action.Should().Throw<FormatException>();
        }
    }
}