using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using WarOfEmpires.CommandHandlers.Alliances;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Alliances {
    [TestClass]
    public sealed class PostChatMessageCommandHandlerTests {
        [TestMethod]
        public void PostChatMessageCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .BuildMember(1);

            var handler = new PostChatMessageCommandHandler(new PlayerRepository(builder.Context));
            var command = new PostChatMessageCommand("test1@test.com", "Test message");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Alliance.Received().PostChatMessage(builder.Player, "Test message");
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void PostChatMessageCommandHandler__Throws_Exception_For_Player_Not_Empire_Member() {
            var builder = new FakeBuilder()
                .WithPlayer(1);

            var handler = new PostChatMessageCommandHandler(new PlayerRepository(builder.Context));
            var command = new PostChatMessageCommand("test1@test.com", "Test message");

            Action action = () => handler.Execute(command);

            action.Should().Throw<NullReferenceException>();
        }
    }
}