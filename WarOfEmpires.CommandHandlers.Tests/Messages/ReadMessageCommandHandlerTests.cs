using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using WarOfEmpires.CommandHandlers.Messages;
using WarOfEmpires.Commands.Messages;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Messages {
    [TestClass]
    public sealed class ReadMessageCommandHandlerTests {
        [TestMethod]
        public void ReadMessageCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var recipient, email: "recipient@test.com")
                .BuildPlayer(2)
                .WithMessageTo(1, out var message, recipient, new DateTime(2020, 1, 1));

            var handler = new ReadMessageCommandHandler(new PlayerRepository(builder.Context));
            var command = new ReadMessageCommand("recipient@test.com", "1");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            message.Received().IsRead = true;
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void ReadMessageCommandHandler_Throws_Exception_For_Message_Received_By_Different_Player() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var recipient, email: "recipient@test.com")
                .BuildPlayer(2, email: "sender@test.com")
                .WithMessageTo(1, out var message, recipient, new DateTime(2020, 1, 1));

            var handler = new ReadMessageCommandHandler(new PlayerRepository(builder.Context));
            var command = new ReadMessageCommand("sender@test.com", "1");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            message.DidNotReceiveWithAnyArgs().IsRead = default;
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void ReadMessageCommandHandler_Throws_Exception_For_Alphanumeric_MessageId() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var recipient, email: "recipient@test.com")
                .BuildPlayer(2)
                .WithMessageTo(1, out var message, recipient, new DateTime(2020, 1, 1));

            var handler = new ReadMessageCommandHandler(new PlayerRepository(builder.Context));
            var command = new ReadMessageCommand("recipient@test.com", "A");

            Action action = () => handler.Execute(command);

            action.Should().Throw<FormatException>();
            message.DidNotReceiveWithAnyArgs().IsRead = default;
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}