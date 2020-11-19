using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using WarOfEmpires.CommandHandlers.Alliances;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Alliances {
    [TestClass]
    public sealed class DeleteChatMessageCommandHandlerTests {
        [TestMethod]
        public void DeleteChatMessageCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithLeader(1, out var leader)
                .WithChatMessage(1, leader, new DateTime(2020, 1, 1), "Test 1")
                .WithChatMessage(2, out var chatMessage, leader, new DateTime(2020, 1, 1), "Test 2");

            var handler = new DeleteChatMessageCommandHandler(new AllianceRepository(builder.Context));
            var command = new DeleteChatMessageCommand("test1@test.com", 2);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Alliance.Received().DeleteChatMessage(chatMessage);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void DeleteChatMessageCommandHandler_Throws_Exception_For_Message_Of_Different_Alliance() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithLeader(1, out var leader)
                .BuildAlliance(2)
                .WithChatMessage(1, leader, new DateTime(2020, 1, 1), "Test 1");

            var handler = new DeleteChatMessageCommandHandler(new AllianceRepository(builder.Context));
            var command = new DeleteChatMessageCommand("test1@test.com", 2);

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            builder.Alliance.DidNotReceiveWithAnyArgs().DeleteChatMessage(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}