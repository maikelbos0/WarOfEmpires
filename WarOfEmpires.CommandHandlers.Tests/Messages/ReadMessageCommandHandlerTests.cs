using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using WarOfEmpires.CommandHandlers.Messages;
using WarOfEmpires.Commands.Messages;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Messages {
    [TestClass]
    public sealed class ReadMessageCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Message _message;

        public ReadMessageCommandHandlerTests() {
            _repository = new PlayerRepository(_context);
            
            var senderUser = Substitute.For<User>();
            senderUser.Email.Returns("sender@test.com");
            senderUser.Status.Returns(UserStatus.Active);

            _message = Substitute.For<Message>();

            var sender = Substitute.For<Player>();
            sender.ReceivedMessages.Returns(new List<Message>());
            sender.SentMessages.Returns(new List<Message>() { _message });
            sender.User.Returns(senderUser);

            _context.Users.Add(senderUser);
            _context.Players.Add(sender);

            var recipientUser = Substitute.For<User>();
            recipientUser.Email.Returns("recipient@test.com");
            recipientUser.Status.Returns(UserStatus.Active);

            var recipient = Substitute.For<Player>();
            recipient.ReceivedMessages.Returns(new List<Message>() { _message });
            recipient.SentMessages.Returns(new List<Message>());
            recipient.User.Returns(recipientUser);

            _message.Id.Returns(1);
            _message.Sender.Returns(sender);
            _message.Recipient.Returns(recipient);

            _context.Users.Add(recipientUser);
            _context.Players.Add(recipient);
        }

        [TestMethod]
        public void ReadMessageCommandHandler_Succeeds() {
            var handler = new ReadMessageCommandHandler(_repository);
            var command = new ReadMessageCommand("recipient@test.com", "1");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _message.Received().IsRead = true;
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void ReadMessageCommandHandler_Throws_Exception_For_Message_Received_By_Different_Player() {
            var handler = new ReadMessageCommandHandler(_repository);
            var command = new ReadMessageCommand("sender@test.com", "1");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            _message.DidNotReceive().IsRead = true;
        }

        [TestMethod]
        public void ReadMessageCommandHandler_Throws_Exception_For_Alphanumeric_MessageId() {
            var handler = new ReadMessageCommandHandler(_repository);
            var command = new ReadMessageCommand("recipient@test.com", "A");

            Action action = () => handler.Execute(command);

            action.Should().Throw<FormatException>();
        }
    }
}