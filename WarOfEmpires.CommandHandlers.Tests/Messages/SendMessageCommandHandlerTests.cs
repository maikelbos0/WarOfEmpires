using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.CommandHandlers.Messages;
using WarOfEmpires.Commands.Messages;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Messages {
    [TestClass]
    public sealed class SendMessageCommandHandlerTests {
        [TestMethod]
        public void SendMessageCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .WithPlayer(2, out var recipient)
                .BuildPlayer(1);

            var handler = new SendMessageCommandHandler(new PlayerRepository(builder.Context));
            var command = new SendMessageCommand("test1@test.com", "2", "Subject", "Body");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.SentMessages.Should().HaveCount(1);
            recipient.ReceivedMessages.Should().HaveCount(1);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void SendMessageCommandHandler_Throws_Exception_For_Alphanumeric_Recipient() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new SendMessageCommandHandler(new PlayerRepository(builder.Context));
            var command = new SendMessageCommand("test1@test.com", "A", "Subject", "Body");

            Action action = () => handler.Execute(command);

            action.Should().Throw<FormatException>();
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SendMessageCommandHandler_Throws_Exception_For_Nonexistent_Recipient() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new SendMessageCommandHandler(new PlayerRepository(builder.Context));
            var command = new SendMessageCommand("test1@test.com", "5", "Subject", "Body");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}