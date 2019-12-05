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
    public sealed class GetReplyToMessageQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();

        public GetReplyToMessageQueryHandlerTests() {
            var senderUser = Substitute.For<User>();
            senderUser.Email.Returns("sender@test.com");
            senderUser.Status.Returns(UserStatus.Active);

            var message = Substitute.For<Message>();

            var sender = Substitute.For<Player>();
            sender.ReceivedMessages.Returns(new List<Message>());
            sender.SentMessages.Returns(new List<Message>() { message });
            sender.User.Returns(senderUser);
            sender.DisplayName.Returns("Sender test");
            sender.Id.Returns(1);

            _context.Users.Add(senderUser);
            _context.Players.Add(sender);

            var recipientUser = Substitute.For<User>();
            recipientUser.Email.Returns("recipient@test.com");
            recipientUser.Status.Returns(UserStatus.Active);

            var recipient = Substitute.For<Player>();
            recipient.ReceivedMessages.Returns(new List<Message>() { message });
            recipient.SentMessages.Returns(new List<Message>());
            recipient.User.Returns(recipientUser);
            recipient.Id.Returns(2);

            message.Id.Returns(1);
            message.Sender.Returns(sender);
            message.Recipient.Returns(recipient);
            message.Subject.Returns("Subject test");
            message.Body.Returns("Body test");
            message.Date.Returns(new DateTime(2019, 1, 5, 14, 15, 33));

            _context.Users.Add(recipientUser);
            _context.Players.Add(recipient);
        }

        [TestMethod]
        public void GetReplyToMessageQueryHandler_Returns_Correct_Information() {
            var handler = new GetReplyToMessageQueryHandler(_context);
            var query = new GetReplyToMessageQuery("recipient@test.com", "1");

            var result = handler.Execute(query);

            result.RecipientId.Should().Be("1");
            result.Recipient.Should().Be("Sender test");
            result.Subject.Should().Be("Re: Subject test");
            result.Body.Should().Be($"{Environment.NewLine}Sender test wrote on 2019-01-05 14:15:{Environment.NewLine}Body test");
        }

        [TestMethod]
        public void GetReplyToMessageQueryHandler_Throws_Exception_For_Alphanumeric_Id() {
            var handler = new GetReplyToMessageQueryHandler(_context);
            var query = new GetReplyToMessageQuery("sender@test.com", "1");

            Action action = () => handler.Execute(query);

            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void GetReplyToMessageQueryHandler_Throws_Exception_For_Nonexistent_Id() {
            var handler = new GetReplyToMessageQueryHandler(_context);
            var query = new GetReplyToMessageQuery("recipient@test.com", "A");

            Action action = () => handler.Execute(query);

            action.Should().Throw<FormatException>();
        }
    }
}