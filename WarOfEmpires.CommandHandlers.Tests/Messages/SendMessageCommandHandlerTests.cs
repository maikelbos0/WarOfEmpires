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
    public sealed class SendMessageCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Player _sender;
        private readonly Player _recipient;

        public SendMessageCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            var senderUser = Substitute.For<User>();
            senderUser.Email.Returns("test@test.com");
            senderUser.Status.Returns(UserStatus.Active);

            _sender = Substitute.For<Player>();
            _sender.Id.Returns(1);
            _sender.SentMessages.Returns(new List<Message>());
            _sender.User.Returns(senderUser);

            _context.Users.Add(senderUser);
            _context.Players.Add(_sender);

            var recipientUser = Substitute.For<User>();
            recipientUser.Email.Returns("recipient@test.com");
            recipientUser.Status.Returns(UserStatus.Active);

            _recipient = Substitute.For<Player>();
            _recipient.Id.Returns(2);
            _recipient.ReceivedMessages.Returns(new List<Message>());
            _recipient.User.Returns(recipientUser);

            _context.Users.Add(recipientUser);
            _context.Players.Add(_recipient);
        }

        [TestMethod]
        public void SendMessageCommandHandler_Succeeds() {
            var handler = new SendMessageCommandHandler(_repository);
            var command = new SendMessageCommand("test@test.com", "2", "Subject", "Body");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _sender.SentMessages.Should().HaveCount(1);
            _recipient.ReceivedMessages.Should().HaveCount(1);
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void SendMessageCommandHandler_Throws_Exception_For_Alphanumeric_Recipient() {
            var handler = new SendMessageCommandHandler(_repository);
            var command = new SendMessageCommand("test@test.com", "A", "Subject", "Body");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void SendMessageCommandHandler_Throws_Exception_For_Nonexistent_Recipient() {
            var handler = new SendMessageCommandHandler(_repository);
            var command = new SendMessageCommand("test@test.com", "5", "Subject", "Body");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
        }
    }
}